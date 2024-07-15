using StockControl.Core.Entities;
using StockControl.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void CreateIfNewStock(Stock newStock);

        StockOperation CreateNewOperation(Stock stock, double buyPrice, int quantity);

        Task<StockHolder?> GetStockHolderByStock(string stockSymbol);

        void CreateStockHolder(StockHolder stockHolder);

        void UpdateStockHolder(StockHolder stockHolder);

        Task Commit();
    }
}
