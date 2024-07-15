using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IStockRepository _stockRepository;
        private readonly IStockOperationRepository _operationRepository;
        private readonly IStockHolderRepository _holderRepository;

        public UnitOfWork(AppDbContext context, IStockRepository stockRepository,
            IStockOperationRepository operationRepository, IStockHolderRepository holderRepository)
        {
            _context = context;
            _stockRepository = stockRepository;
            _operationRepository = operationRepository;
            _holderRepository = holderRepository;
        }

        public void CreateIfNewStock(Stock newStock)
        {
            var stock = _stockRepository.GetAsync(newStock.Symbol).GetAwaiter().GetResult();

            if (stock != null)
                return;

            _stockRepository.Create(newStock);
        }

        public void CreateStockHolder(StockHolder stockHolder)
        {
            _holderRepository.Create(stockHolder);
        }

        public void UpdateStockHolder(StockHolder stockHolder)
        {
            _holderRepository.Update(stockHolder);
        }

        public StockOperation CreateNewOperation(Stock stock, double buyPrice, int quantity)
        {
            var stockOperation = new StockOperation
            {
                Date = DateTime.Now,
                OperatingType = OperationType.Buy,
                Quantity = quantity,
                Price = buyPrice,
                StockSymbol = stock.Symbol,
            };

            _operationRepository.Create(stockOperation);

            return stockOperation;
        }

        public async Task<StockHolder?> GetStockHolderByStock(string stockSymbol)
        {
            return await _holderRepository.GetHolderByStock(stockSymbol);
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
