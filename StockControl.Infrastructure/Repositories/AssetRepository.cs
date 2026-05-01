using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class AssetRepository(AppDbContext context) : BaseRepository<Asset>(context), IAssetRepository
    {
        public async Task<Asset?> GetAsync(string ticker)
        {
            return await _context.Assets.FirstOrDefaultAsync(x => x.Ticker.Equals(ticker));
        }

        public DateTime GetLatestUpdate()
        {
            return _context.Assets
              .AsNoTracking()
              .Max(e => e.LastUpdate);
        }
    }
}
