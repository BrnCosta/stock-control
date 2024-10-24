using StockControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
  public interface IStockHolderService
  {
    List<object> GetAllStockHolders();

    StockHolder BuyStock(string stockSymbol, int buyQuantity, double buyPrice);

    StockHolder SellStock(string stockSymbol, int sellQuantity, double sellPrice);
  }
}
