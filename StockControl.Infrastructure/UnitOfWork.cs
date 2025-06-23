using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure
{
  public class UnitOfWork(AppDbContext context, IStockRepository stockRepository,
        IStockOperationRepository operationRepository, IStockHolderRepository holderRepository, 
        IDividendRepository dividendRepository) : IUnitOfWork, IDisposable
  {
    private readonly AppDbContext _context = context;
    private readonly IStockRepository _stockRepository = stockRepository;
    private readonly IStockOperationRepository _operationRepository = operationRepository;
    private readonly IStockHolderRepository _holderRepository = holderRepository;
    private readonly IDividendRepository _dividendRepository = dividendRepository;

    public IStockRepository StockRepository { get { return _stockRepository; } }
    public IStockOperationRepository OperationRepository { get { return _operationRepository; } }
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
