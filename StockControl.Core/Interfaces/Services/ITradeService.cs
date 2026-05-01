using StockControl.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
    public interface ITradeService
    {
        Task<Guid> CreateNewTrade(TradeRequest tradeRequest);
    }
}
