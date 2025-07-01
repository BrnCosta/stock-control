using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Requests;

namespace StockControl.Core.Interfaces.Services
{
  public interface ITransactionService
  {
    List<Transaction> GetAllTransactions();
    List<StockOperation> GetAllOperationsFromTransaction(int transactionId);
    void UpdateTax(UpdateTaxRequest updateRequest);
    void CreateNewTransaction(TransactionRequest transaction);
  }
}
