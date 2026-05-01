using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Services
{
    public interface IAssetService
    {
        Asset GetOrCreateNewAsset(string ticker);
        DateTime GetLatestUpdate();
    }
}
