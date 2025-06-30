using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
using StockControl.Core.Responses;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class StockController(IStockHolderService holderService, IStockService stockService) : ControllerBase
  {
    private readonly IStockHolderService _stockHolderService = holderService;
    private readonly IStockService _stockService = stockService;

    [HttpGet]
    public List<StockHolderOverviewResponse> GetStockHolders()
    {
      return _stockHolderService.GetStockHoldersOverview();
    }

    [HttpGet("latest-update")]
    public DateTime GetLatestUpdate()
    {
      return _stockService.GetLatestUpdate();
    }
  }
}
