using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Responses;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
    public class DividendRepository(AppDbContext context) : BaseRepository<Dividend>(context), IDividendRepository 
    {
        public new IEnumerable<Dividend> GetAll()
        {
            return _context.Dividends
                .Include(d => d.Asset)
                .AsNoTracking();
        }
    }
}
