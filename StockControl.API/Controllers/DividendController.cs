using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

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

    [HttpPost]
    public IActionResult CreateNewDividend([FromBody] DividendRequest request)
    {
      _dividendService.CreateNewDividend(request.StockSymbol, request.Value, request.Date);

      return Created();
    }
  }
}
