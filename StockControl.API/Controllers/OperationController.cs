using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class OperationController(IOperationService operationService) : ControllerBase
  {
    private readonly IOperationService _operationService = operationService;

    [HttpGet]
    public List<StockOperation> GetStockHolders()
    {
      return _operationService.GetAllOperations();
    }
  }
}
