using StockControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Repositories
{
    public interface IStockRepository : IBaseRepository<Stock>
    {
        Task<Stock?> GetAsync(string symbol);
    }
}
