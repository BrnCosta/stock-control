using StockControl.Core.Entities;
using StockControl.Core.Requests;
using StockControl.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Interfaces.Services
{
    public interface ITradeService
    {
        Task<List<TradeOverviewResponse>> GetAllTrades();
        Task<Guid> CreateNewTrade(TradeRequest tradeRequest);
    }
}
