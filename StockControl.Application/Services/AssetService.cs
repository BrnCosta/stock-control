using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Interfaces.Services.External;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
    public class AssetService(IUnitOfWork unitOfWork, IAssetInformationService assetInformationService) : IAssetService
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IAssetInformationService _assetInformationService = assetInformationService;

        public DateTime GetLatestUpdate()
        {
            return _unitOfWork.AssetRepository.GetLatestUpdate();
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
            IEnumerable<Asset> assets = _unitOfWork.AssetRepository.GetAll();

            var updateTime = DateTime.UtcNow;

            foreach (var asset in assets)
            {
                decimal currentPrice = await _assetInformationService.GetAssetMarketPrice(asset.Ticker);
                asset.Price = currentPrice;
                asset.LastUpdate = updateTime;
                _unitOfWork.AssetRepository.Update(asset);
            }

            await _unitOfWork.Commit();

            return updateTime;
        }

        private async Task<Asset> CreateNewAsset(string ticker, Currency currency)
        {
            AssetInformationResponse stockInformation = await _assetInformationService.GetAssetInformationResultAsync(ticker);

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
    }
}
