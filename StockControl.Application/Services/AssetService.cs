using Microsoft.Extensions.DependencyInjection;
using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Interfaces.Services.External;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
    public class AssetService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : IAssetService
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IServiceProvider _serviceProvider = serviceProvider;

        public DateTime GetLatestUpdate()
        {
            return _unitOfWork.AssetRepository.GetLatestUpdate();
        }

        public IEnumerable<string> GetRegisteredTickers()
        {
            return _unitOfWork.AssetRepository.GetRegisteredTickers();
        }

        public async Task<Asset?> GetAssetByTicker(string ticker)
        {
            return await _unitOfWork.AssetRepository.GetAsync(ticker);
        }

        public async Task<Asset> GetOrCreateNewAsset(string ticker, Currency currency)
        {
            Asset? asset = await _unitOfWork.AssetRepository.GetAsync(ticker);

            if (asset is null)
                return await CreateNewAsset(ticker, currency);

            return asset;
        }

        public async Task<DateTime> UpdateAssetPrice()
        {
            IEnumerable<Position> positions = _unitOfWork.PositionRepository.GetAll();

            var updateTime = DateTime.Now;

            foreach (var position in positions)
            {
                var asset = _unitOfWork.AssetRepository.GetAsync(position.Asset.Ticker).Result
                    ?? throw new InvalidOperationException($"Asset with Ticker {position.Asset.Ticker} not found.");

                var assetInformationService = await GetRequiredAssetInformationService(asset.Currency)
                    ?? throw new InvalidOperationException($"No asset information service available for the currency {asset.Currency}.");

                decimal currentPrice = await assetInformationService.GetAssetMarketPrice(asset.Ticker);
                asset.Price = currentPrice;
                asset.LastUpdate = updateTime;
                _unitOfWork.AssetRepository.Update(asset);
            }

            await _unitOfWork.Commit();

            return updateTime;
        }

        private async Task<Asset> CreateNewAsset(string ticker, Currency currency)
        {
            var assetInformationService = await GetRequiredAssetInformationService(currency)
                ?? throw new InvalidOperationException($"No asset information service available for the currency {currency}.");

            AssetInformationResponse stockInformation = await assetInformationService.GetAssetInformationResultAsync(ticker);

            var asset = new Asset
            {
                Ticker = ticker,
                Price = stockInformation.RegularMarketPrice,
                Type = stockInformation.Type,
                LastUpdate = DateTime.Now,
                Currency = currency
            };

            _unitOfWork.AssetRepository.Create(asset);

            return asset;
        }

        private async Task<IAssetInformationService?> GetRequiredAssetInformationService(Currency currency)
        {
            return _serviceProvider.GetKeyedService<IAssetInformationService>(currency);
        }
    }
}
