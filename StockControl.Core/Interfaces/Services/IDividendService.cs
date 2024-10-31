using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
  public interface IDividendService
  {
    void CreateNewDividend(string stockSymbol, double value, DateOnly date);
  }
}
