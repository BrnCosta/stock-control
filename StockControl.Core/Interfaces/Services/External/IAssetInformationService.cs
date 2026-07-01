using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Responses;
using StockControl.Core.Responses.External;

namespace StockControl.Core.Interfaces.Services.External
{
    public interface IAssetInformationService
    {
        Task<AssetInformationResponse> GetAssetInformationResultAsync(string ticker);
        Task<decimal> GetAssetMarketPrice(string ticker);
    }
}
