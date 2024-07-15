using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class StockRepository(AppDbContext context) : BaseRepository<Stock>(context), IStockRepository
    {
        public async Task<Stock?> GetAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol.Equals(symbol));
        }
    }
}
