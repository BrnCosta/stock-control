using StockControl.Core.Entities;
using StockControl.Core.Responses.Position;

namespace StockControl.Core.Interfaces.Repositories
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        Task<Position?> GetPositionByAssetTicket(string ticker);
    }
}
