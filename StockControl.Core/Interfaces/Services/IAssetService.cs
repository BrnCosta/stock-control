using StockControl.Core.Entities;
using StockControl.Core.Enums;

namespace StockControl.Core.Interfaces.Services
{
    public interface IAssetService
    {
        Asset GetOrCreateNewAsset(string ticker, Currency currency);
        DateTime GetLatestUpdate();
    }
}
