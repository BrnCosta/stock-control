using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class PositionRepository(AppDbContext context) : BaseRepository<Position>(context), IPositionRepository
    {
        public Task<Position?> GetPositionByAssetTicket(string ticker)
        {
            return _context.Positions.FirstOrDefaultAsync(x => x.Asset.Ticker == ticker);
        }

        public new IEnumerable<Position> GetAll()
        {
            return _context.Positions.Include(x => x.Asset).AsNoTracking();
        }
    }
}
