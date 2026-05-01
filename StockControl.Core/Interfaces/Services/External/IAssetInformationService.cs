using StockControl.Core.Enums;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Services.External
{
    public interface IAssetInformationService
    {
        Task<AssetInformationResponse> GetAssetInformationResultAsync(string ticker);
        Task<AssetType> GetAssetType(string ticker);
        Task<decimal> GetAssetMarketPrice(string ticker);
    }
}
