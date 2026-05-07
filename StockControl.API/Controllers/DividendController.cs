using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> CreateNewDividend(DividendRequest dividendRequest)
        {
            var dividendId = await _dividendService.CreateAsync(dividendRequest);
            return CreatedAtAction(nameof(CreateNewDividend), new { id = dividendId }, dividendId);
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
