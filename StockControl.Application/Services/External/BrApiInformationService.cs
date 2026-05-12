using StockControl.Core.Enums;
using StockControl.Core.Interfaces.Services.External;
using StockControl.Core.Responses;
using StockControl.Core.Responses.BrApi;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;

namespace StockControl.Application.Services.External
{
    public class BrApiInformationService(IHttpClientFactory httpClient) : IAssetInformationService
    {
        private const string BRAPI_QUOTE_URL = "https://brapi.dev/api/quote/";
        private const string BRAPI_LIST_SEARCH_URL = BRAPI_QUOTE_URL + "list?search=";
        private const string BRAPI_INTERVAL_PARAMS = "?range=1d&interval=1d";
        private readonly string? AUTHENTICATION_TOKEN = Environment.GetEnvironmentVariable("BRAPI_AUTHENTICATION_TOKEN");

        private readonly IHttpClientFactory _httpClient = httpClient;

        public async Task<AssetInformationResponse> GetAssetInformationResultAsync(string ticker)
        {
            var assetInformationResult = new AssetInformationResponse()
            {
                Type = await GetAssetType(ticker),
                RegularMarketPrice = await GetAssetMarketPrice(ticker)
            };

            return assetInformationResult;
        }

        public async Task<AssetType> GetAssetType(string ticker)
        {
            string apiUrl = string.Concat(BRAPI_LIST_SEARCH_URL, ticker);

            var jsonResponse = await ApiGetAsync<BrApiResponse>(apiUrl);

            string brApiType = jsonResponse?.Stocks?.FirstOrDefault()?.Type ?? throw new Exception("Cannot retrieve asset type.");

            return brApiType switch
            {
                "stock" => AssetType.STOCK,
                "fund" => AssetType.REIT,
                _ => throw new Exception("Unknown asset type.")
            };
        }

        public async Task<decimal> GetAssetMarketPrice(string ticker)
        {
            try
            {
                string apiUrl = string.Concat(BRAPI_QUOTE_URL, ticker, BRAPI_INTERVAL_PARAMS);

                var jsonResponse = await ApiGetAsync<BrApiResponse>(apiUrl);

                return jsonResponse?.Results?.FirstOrDefault()?.RegularMarketPrice ?? throw new Exception($"Cannot retrieve market price for {ticker}.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving market price for {ticker}.", ex);
            }
        }

        private async Task<T?> ApiGetAsync<T>(string apiUrl)
        {
            if (AUTHENTICATION_TOKEN is null)
                throw new InvalidCredentialException("Failed to get BRAPI Authentication Token.");

            var httpClient = _httpClient.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AUTHENTICATION_TOKEN);

            var response = (await httpClient.GetAsync(apiUrl)).EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
