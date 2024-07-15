using StockControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Repositories
{
    public interface IStockHolderRepository : IBaseRepository<StockHolder>
    {
        Task<StockHolder?> GetHolderByStock(string stockSymbol);
    }
}
