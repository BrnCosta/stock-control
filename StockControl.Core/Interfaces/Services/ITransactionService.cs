using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Requests;

namespace StockControl.Core.Interfaces.Services
{
    public interface ITransactionService
    {
        Transaction CreateNewTransaction(TransactionRequest transactionRequest, Asset asset, Trade trade);
    }
}
