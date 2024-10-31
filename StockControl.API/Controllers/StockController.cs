using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
using StockControl.Core.Responses;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StockController(IStockHolderService stockService) : ControllerBase
  {
    private readonly IStockHolderService _stockHolderService = stockService;

    [HttpGet]
    public List<StockHolderOverviewResponse> GetStockHolders()
    {
      return _stockHolderService.GetStockHoldersOverview();
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
