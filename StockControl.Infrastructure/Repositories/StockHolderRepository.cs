using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Infrastructure.Repositories
{
    public class StockHolderRepository(AppDbContext context) : BaseRepository<StockHolder>(context), IStockHolderRepository
    {
        public Task<StockHolder?> GetHolderByStock(string stockSymbol)
        {
            return _context.StockHolders.FirstOrDefaultAsync(x => x.StockSymbol == stockSymbol);
        }
    }
}
