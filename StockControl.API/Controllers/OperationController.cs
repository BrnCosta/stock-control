using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class OperationController(IOperationService operationService) : ControllerBase
  {
    private readonly IOperationService _operationService = operationService;

    [HttpGet]
    public List<StockOperation> GetAllOperations()
    {
      return _operationService.GetAllOperations();
    }
  }
}
