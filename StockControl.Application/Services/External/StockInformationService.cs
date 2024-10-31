using StockControl.Core.Interfaces.Services.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Application.Services.External
{
  public class StockInformationService(IHttpClientFactory httpClient) : IStockInformationService
  {
    private const string BRAPI_QUOTE_URL = "https://brapi.dev/api/quote/";
    private const string BRAPI_INTERVAL_PARAMS = "?range=1d&interval=1d";
    private const string AUTHENTICATION_TOKEN = "13txL2WCFqDiS9eGn9gUQF";

    private readonly IHttpClientFactory _httpClient = httpClient;

    private class BrApiResponse
    {
      public required List<StockInformationResult> Results { get; set; }
    }

    public async Task<StockInformationResult> GetStockInformation(string stockSymbol)
    {
      string apiUrl = String.Concat(BRAPI_QUOTE_URL, stockSymbol, BRAPI_INTERVAL_PARAMS);

      var httpClient = _httpClient.CreateClient();
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AUTHENTICATION_TOKEN);

      var response = (await httpClient.GetAsync(apiUrl)).EnsureSuccessStatusCode();

      var jsonResponse = await response.Content.ReadFromJsonAsync<BrApiResponse>();

      StockInformationResult stockResult = jsonResponse?.Results.FirstOrDefault() ?? throw new Exception("Cannot retrieve stock information.");

      return stockResult;
    }
  }
}
