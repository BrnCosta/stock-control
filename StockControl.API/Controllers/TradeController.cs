using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController(ITradeService tradeService) : ControllerBase
    {
        private readonly ITradeService _tradeService = tradeService;

        [HttpPost]
        public async Task<IActionResult> CreateNewTrade([FromBody] TradeRequest tradeRequest)
        {
            var tradeId = await _tradeService.CreateNewTrade(tradeRequest);
            return Created(nameof(CreateNewTrade), tradeId);
        }
    }
}
