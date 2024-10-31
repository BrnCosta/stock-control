using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
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
    public List<DividendGroupBySymbolResponse> GetGroupBySymbol()
    {
      return _dividendService.GetGroupBySymbol();
    }

    [HttpPost]
    public IActionResult CreateNewDividend([FromBody] DividendRequest request)
    {
      _dividendService.CreateNewDividend(request.StockSymbol, request.Value, request.Date);

      return Created();
    }
  }
}
