using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Interfaces.Services;

namespace StockControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PositionController(IPositionService holderService) : ControllerBase
    {
        private readonly IPositionService _positionService = holderService;

        [HttpGet]
        public async Task<IActionResult> GetCurrentPosition()
        {
            return Ok(_positionService.GetAllCurrentPosition());
        }
    }
}
