using StockControl.Core.Entities;
using StockControl.Core.Responses.Position;

namespace StockControl.Core.Interfaces.Services
{
    public interface IPositionService
    {
        IEnumerable<PositionAssetOverviewResponse> GetAllAssetCurrentPosition();
        IEnumerable<PositionBalanceResponse> GetBalanceOverall();
        IEnumerable<AssetTypeOverviewResponse> GetOverview();
        Position CreateOrUpdatePosition(Asset asset, Transaction transaction);
    }
}
