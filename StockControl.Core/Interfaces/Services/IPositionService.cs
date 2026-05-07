using StockControl.Core.Entities;
using StockControl.Core.Responses.Position;

namespace StockControl.Core.Interfaces.Services
{
    public interface IPositionService
    {
        IEnumerable<PositionAssetOverviewResponse> GetAllAssetCurrentPosition();
        PositionBalanceResponse GetBalanceOverall();
        Position CreateOrUpdatePosition(Asset asset, Transaction transaction);
        Task<PositionWalletOverviewResponse> GetOverview();
    }
}
