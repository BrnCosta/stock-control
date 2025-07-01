using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;
using StockControl.Infrastructure.Repositories;

namespace StockControl.Infrastructure
{
  public class UnitOfWork(AppDbContext context, IStockRepository stockRepository,
        ITransactionRepository transactionRepository, IStockHolderRepository holderRepository, 
        IDividendRepository dividendRepository) : IUnitOfWork, IDisposable
  {
    private readonly AppDbContext _context = context;
    private readonly IStockRepository _stockRepository = stockRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IStockHolderRepository _holderRepository = holderRepository;
    private readonly IDividendRepository _dividendRepository = dividendRepository;

    public IStockRepository StockRepository { get { return _stockRepository; } }
    public ITransactionRepository TransactionRepository { get { return _transactionRepository; } }
    public IStockHolderRepository StockHolderRepository { get { return _holderRepository; } }
    public IDividendRepository DividendRepository { get { return _dividendRepository; } }

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
