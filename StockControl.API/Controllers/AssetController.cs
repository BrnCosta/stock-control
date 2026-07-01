using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
using StockControl.Core.Responses;

namespace StockControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetController(IAssetService assetService) : ControllerBase
    {
        private readonly IAssetService _assetService = assetService;

        [HttpGet]
        public async Task<IActionResult> GetRegisteredAssets()
        {
            try
            {
                return Ok(_assetService.GetRegisteredTickers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message, ex.InnerException });
            }
        }

        [HttpGet("latest-update")]
        public DateTime GetLatestUpdate()
        {
            return _assetService.GetLatestUpdate();
        }

        [HttpGet("update")]
        public async Task<IActionResult> UpdateAsset()
        {
            try
            {
                DateTime updateTime = await _assetService.UpdateAssetPrice();
                return Ok(updateTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message, ex.InnerException });
            }
        }
    }
}
