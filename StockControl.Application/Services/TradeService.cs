using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.Application.Services
{
    public class TradeService(IUnitOfWork unitOfWork, IPositionService positionService, IAssetService assetService, ITransactionService transactionService) : ITradeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPositionService _positionService = positionService;
        private readonly IAssetService _assetService = assetService;
        private readonly ITransactionService _transactionService = transactionService;

        public async Task<Guid> CreateNewTrade(TradeRequest tradeRequest)
        {
            if(tradeRequest.Transactions == null || tradeRequest.Transactions.Count == 0)
                throw new ArgumentException("Trade must have at least one transaction.");

            var validCurrency = ValidateTradeCurrency(tradeRequest.Currency);

            var trade = new Trade
            {
                Id = Guid.NewGuid(),
                Tax = tradeRequest.Tax,
                Date = tradeRequest.Date,
                Currency = validCurrency
            };

            foreach (var transactionRequest in tradeRequest.Transactions)
            {
                if(string.IsNullOrEmpty(transactionRequest.Ticker))
                    throw new ArgumentException("Transaction must have a ticker.");

                var asset = _assetService.GetOrCreateNewAsset(transactionRequest.Ticker, validCurrency);

                var transaction = _transactionService.CreateNewTransaction(transactionRequest, asset, trade);

                _positionService.CreateOrUpdatePosition(asset, transaction);
    
                trade.Transactions.Add(transaction);
            }

            _unitOfWork.TradeRepository.Create(trade);
            await _unitOfWork.Commit();

            return trade.Id;
        }

        private static Currency ValidateTradeCurrency(string currencyRequested)
        {
            if (string.IsNullOrEmpty(currencyRequested))
                throw new ArgumentException("Trade must have a currency.");

            if(!Enum.TryParse<Currency>(currencyRequested, out var validCurrency))
                throw new ArgumentException("Trade must have a valid currency.");

            return validCurrency;
        }
    }
}
