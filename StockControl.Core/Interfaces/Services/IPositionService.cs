using StockControl.Core.Entities;
using StockControl.Core.Responses;

namespace StockControl.Core.Interfaces.Services
{
    public interface IPositionService
    {
        IEnumerable<PositionAssetOverviewResponse> GetAllAssetCurrentPosition();
        PositionBalance GetBalanceOverall();
        Position CreateOrUpdatePosition(Asset asset, Transaction transaction);
    }
}
