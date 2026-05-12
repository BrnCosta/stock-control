using StockControl.Core.Entities;
using StockControl.Core.Enums;

namespace StockControl.Core.Interfaces.Services
{
    public interface IAssetService
    {
        Task<Asset?> GetAssetByTicker(string ticker);
        Task<Asset> GetOrCreateNewAsset(string ticker, Currency currency);
        DateTime GetLatestUpdate();
        Task<DateTime> UpdateAssetPrice();
    }
}
