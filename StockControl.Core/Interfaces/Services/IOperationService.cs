using StockControl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
  public interface IOperationService
  {
    void CreateNewOperation(Stock stock, double buyPrice, int quantity);
  }
}
