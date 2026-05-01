using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;
using StockControl.Infrastructure.Repositories;

namespace StockControl.Infrastructure
{
    public class UnitOfWork(AppDbContext context, IAssetRepository assetRepository,
          ITradeRepository tradeRepository, IPositionRepository positionRepository,
          IDividendRepository dividendRepository, ITransactionRepository transactionRepository) : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context = context;

        private readonly IAssetRepository _assetRepository = assetRepository;
        private readonly ITradeRepository _tradeRepository = tradeRepository;
        private readonly IPositionRepository _positionRepository = positionRepository;
        private readonly IDividendRepository _dividendRepository = dividendRepository;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;

        public IAssetRepository AssetRepository { get { return _assetRepository; } }
        public ITradeRepository TradeRepository { get { return _tradeRepository; } }
        public IPositionRepository PositionRepository { get { return _positionRepository; } }
        public IDividendRepository DividendRepository { get { return _dividendRepository; } }
        public ITransactionRepository TransactionRepository { get { return _transactionRepository; } }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
