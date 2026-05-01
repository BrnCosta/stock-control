using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Responses;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class DividendController(IDividendService dividendService) : ControllerBase
  {
    private readonly IDividendService _dividendService = dividendService;

    [HttpGet]
    public List<Dividend> GetAll()
    {
      return _dividendService.GetAll();
    }

    [HttpGet("by-month")]
    public List<DividendGroupByMonthResponse> GetGroupByMonth()
    {
      return _dividendService.GetGroupByMonth();
    }

    [HttpGet("by-symbol")]
    public List<DividendGroupByTickerResponse> GetGroupBySymbol()
    {
      return _dividendService.GetGroupByTicker();
    }
  }
}
