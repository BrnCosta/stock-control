using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces
{
  public interface IUnitOfWork
  {
    IStockRepository StockRepository { get; }
    IStockOperationRepository OperationRepository { get; }
    IStockHolderRepository StockHolderRepository { get; }

    Task Commit();
  }
}
