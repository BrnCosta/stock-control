using StockControl.Core.Enums;
using StockControl.Core.Interfaces.Services.External;
using StockControl.Core.Responses;
using YahooFinanceApi;

namespace StockControl.Application.Services.External
{
    public class YahooFinanceInformationService() : IAssetInformationService
    {
        public async Task<AssetInformationResponse> GetAssetInformationResultAsync(string ticker)
        {
            var assetInformationResult = new AssetInformationResponse()
            {
                Type = await GetAssetType(ticker),
                RegularMarketPrice = await GetAssetMarketPrice(ticker)
            };

            return assetInformationResult;
        }

        private async Task<AssetType> GetAssetType(string ticker)
        {
            var tsxTicker = $"{ticker}.TO";
            string quoteType = await GetTsxQuoteType(tsxTicker);

            return quoteType switch
            {
                "EQUITY" => AssetType.STOCK,
                "ETF" => AssetType.ETF,
                _ => throw new Exception("Unknown asset type.")
            };
        }

        private async static Task<string> GetTsxQuoteType(string tsxTicker)
        {
            try
            {
                var symbols = new[] { tsxTicker };
                var quotes = await Yahoo.Symbols(symbols)
                    .Fields(Field.QuoteType)
                    .QueryAsync();

                var quoteType = quotes[tsxTicker]?.QuoteType ?? throw new Exception($"Cannot retrieve quote type for {tsxTicker}.");

                return quoteType;
            }
            catch (Exception ex)
            {
                throw new Exception($"Yahoo API Error: {ex.Message}", ex);
            }
        }

        public async Task<decimal> GetAssetMarketPrice(string ticker)
        {
            try
            {
                var tsxTicker = $"{ticker}.TO";
                var symbols = new[] { tsxTicker };

                var quotes = await Yahoo.Symbols(symbols)
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();

                var regularMarketPrice = quotes[tsxTicker]?.RegularMarketPrice ?? throw new Exception($"Cannot retrieve market price for {tsxTicker}.");

                return (decimal) regularMarketPrice;
            }
            catch (Exception ex)
            {
                throw new Exception($"Yahoo API Error: {ex.Message}", ex);
            }
        }
    }
}
