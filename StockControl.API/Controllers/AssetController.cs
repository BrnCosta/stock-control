using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
using StockControl.Core.Responses;

namespace StockControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetController(IAssetService stockService) : ControllerBase
    {
        private readonly IAssetService _assetService = stockService;

        [HttpGet("latest-update")]
        public DateTime GetLatestUpdate()
        {
            return _assetService.GetLatestUpdate();
        }
    }
}
