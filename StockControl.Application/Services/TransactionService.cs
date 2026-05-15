using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.Application.Services
{
    public class TransactionService(IUnitOfWork unitOfWork, IAssetService stockService, IPositionService holderService) : ITransactionService
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IAssetService _stockService = stockService;
        protected readonly IPositionService _holderService = holderService;

        public Transaction CreateNewTransaction(TransactionRequest transactionRequest, Asset asset, Trade trade)
        {
            var validOperationType = ValidateOperationType(transactionRequest.OperatingType);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Quantity = transactionRequest.Quantity,
                Price = transactionRequest.Price,
                OperatingType = validOperationType,
                Asset = asset,
                Trade = trade,
            };

            _unitOfWork.TransactionRepository.Create(transaction);

            return transaction;
        }

        private static OperationType ValidateOperationType(string operationType)
        {
            if (Enum.TryParse(operationType, out OperationType result))
            {
                return result;
            }

            throw new ArgumentException($"Invalid operation type: {operationType}");
        }
    }
}
