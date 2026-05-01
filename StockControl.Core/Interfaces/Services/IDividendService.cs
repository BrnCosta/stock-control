using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
  public interface IDividendService
  {
    List<Dividend> GetAll();
    List<DividendGroupByMonthResponse> GetGroupByMonth();
    List<DividendGroupByTickerResponse> GetGroupByTicker();
  }
}
