using StockControl.Core.Interfaces.Repositories;

namespace StockControl.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IAssetRepository AssetRepository { get; }
        ITradeRepository TradeRepository { get; }
        IPositionRepository PositionRepository { get; }
        IDividendRepository DividendRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        Task Commit();
    }
}
