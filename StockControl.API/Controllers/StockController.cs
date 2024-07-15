using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Core.Entities;
using StockControl.Core.Requests;

namespace StockControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(StockService stockService) : ControllerBase
    {
        private readonly StockService _stockService = stockService;

        [HttpPost("buy")]
        public StockHolder BuyStock([FromBody] BuyStockRequest buyRequest)
        {
            return _stockService.BuyNewStock(buyRequest.StockSymbol, buyRequest.Quantity, buyRequest.Price);
        }
    }
}
