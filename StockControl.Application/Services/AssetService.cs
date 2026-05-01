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

        public Asset GetOrCreateNewAsset(string ticker)
        {
            Asset? asset = _unitOfWork.AssetRepository.GetAsync(ticker).GetAwaiter().GetResult();

            if (asset is null)
                return CreateNewAsset(ticker);

            return asset;
        }

        private Asset CreateNewAsset(string ticker)
        {
            AssetInformationResponse stockInformation = _assetInformationService.GetAssetInformationResultAsync(ticker).GetAwaiter().GetResult();

            var asset = new Asset
            {
                Ticker = ticker,
                Price = stockInformation.RegularMarketPrice,
                Type = stockInformation.Type,
                LastUpdate = DateTime.Now,
            };

            _unitOfWork.AssetRepository.Create(asset);

            return asset;
        }
    }
}
