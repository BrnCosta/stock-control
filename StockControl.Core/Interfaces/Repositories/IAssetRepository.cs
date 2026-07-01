using StockControl.Core.Entities;

namespace StockControl.Core.Interfaces.Repositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        Task<Asset?> GetAsync(string ticker);
        DateTime GetLatestUpdate();
        IEnumerable<string> GetRegisteredTickers();
    }
}
