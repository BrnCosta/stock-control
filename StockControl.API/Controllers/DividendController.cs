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

    [HttpPost]
    public IActionResult CreateNewDividend([FromBody] DividendRequest request)
    {
      _dividendService.CreateNewDividend(request.StockSymbol, request.Value, DateOnly.FromDateTime(request.Date));

      return Created();
    }
  }
}
