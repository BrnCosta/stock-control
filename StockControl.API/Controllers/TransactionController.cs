using Microsoft.AspNetCore.Mvc;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TransactionController(ITransactionService transactionService) : ControllerBase
  {
    private readonly ITransactionService _transactionService = transactionService;

    [HttpGet]
    [Route("all")]
    public List<Transaction> GetAllTransactions()
    {
      return _transactionService.GetAllTransactions();
    }

    [HttpGet]
    [Route("all-operations")]
    public List<StockOperation> GetAllOperationsFromTransaction(int transactionId)
    {
      return _transactionService.GetAllOperationsFromTransaction(transactionId);
    }

    [HttpPost]
    public IActionResult AddTransaction([FromBody] TransactionRequest transaction)
    {
      _transactionService.CreateNewTransaction(transaction);
      return Created();
    }

    [HttpPut]
    [Route("update-tax")]
    public IActionResult UpdateTax([FromBody] UpdateTaxRequest updateRequest)
    {
      _transactionService.UpdateTax(updateRequest);
      return Ok();
    }
  }
}
