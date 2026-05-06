using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
    public class DividendService(IUnitOfWork unitOfWork, IAssetService assetService) : IDividendService
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAssetService _assetService = assetService;

        public async Task<Guid> CreateAsync(DividendRequest dividendRequest)
        {
            if(dividendRequest == null)
                throw new ArgumentNullException(nameof(dividendRequest));

            var asset = await _assetService.GetAssetByTicker(dividendRequest.Ticker) ??
                throw new ArgumentException($"There's no asset with ticker {dividendRequest.Ticker}. Dividend should only be registered for existing assets.");

            var dividend = new Dividend
            {
                Id = Guid.NewGuid(),
                Asset = asset,
                Value = dividendRequest.Value,
                Date = dividendRequest.Date
            };

            _unitOfWork.DividendRepository.Create(dividend);
            await _unitOfWork.Commit();

            return dividend.Id;
        }

        public List<Dividend> GetAll()
        {
            return _unitOfWork.DividendRepository.GetAll().ToList();
        }

        public List<DividendGroupByMonthResponse> GetGroupByMonth()
        {
            var allDividends = _unitOfWork.DividendRepository.GetAll();

            var groupByMonth = allDividends
              .GroupBy(d => new { d.Date.Year, d.Date.Month })
              .Select(g => new DividendGroupByMonthResponse
              {
                  Year = g.Key.Year,
                  Month = g.Key.Month,
                  TotalValue = g.Sum(d => d.Value)
              })
              .OrderBy(summary => summary.Year)
              .ThenBy(summary => summary.Month);

            return groupByMonth.ToList();
        }

        public List<DividendGroupByTickerResponse> GetGroupByTicker()
        {
            var allDividends = _unitOfWork.DividendRepository.GetAll();

            var groupByTicker = allDividends
              .GroupBy(d => new { d.Asset.Ticker, d.Date.Year, d.Date.Month })
              .Select(g => new DividendGroupByTickerResponse
              {
                  Year = g.Key.Year,
                  Month = g.Key.Month,
                  TotalValue = g.Sum(d => d.Value),
                  Asset = g.Key.Ticker
              })
              .OrderBy(summary => summary.Year)
              .ThenBy(summary => summary.Month)
              .ThenBy(summary => summary.Asset);

            return groupByTicker.ToList();
        }
    }
}
