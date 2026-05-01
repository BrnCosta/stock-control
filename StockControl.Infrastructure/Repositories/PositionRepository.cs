using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Responses;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class PositionRepository(AppDbContext context) : BaseRepository<Position>(context), IPositionRepository
    {
        public Task<Position?> GetPositionByAssetTicket(string ticker)
        {
            return _context.Positions.FirstOrDefaultAsync(x => x.Asset.Ticker == ticker);
        }
    }
}
