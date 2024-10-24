using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Core.Entities;
using StockControl.Core.Requests;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StockController(StockHolderService stockService) : ControllerBase
  {
    private readonly StockHolderService _stockHolderService = stockService;

    [HttpGet("holder")]
    public List<object> GetStockHolders()
    {
      return _stockHolderService.GetAllStockHolders();
    }

    [HttpPost("buy")]
    public StockHolder BuyStock([FromBody] OperationStockRequest buyRequest)
    {
      return _stockHolderService.BuyStock(buyRequest.StockSymbol, buyRequest.Quantity, buyRequest.Price);
    }

    [HttpPut("sell")]
    public StockHolder SellStock([FromBody] OperationStockRequest sellRequest)
    {
      return _stockHolderService.SellStock(sellRequest.StockSymbol, sellRequest.Quantity, sellRequest.Price);
    }
  }
}
