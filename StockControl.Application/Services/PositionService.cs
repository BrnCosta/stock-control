using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Responses.Position;

namespace StockControl.Application.Services
{
    public class PositionService(IUnitOfWork unitOfWork) : IPositionService
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IEnumerable<PositionAssetOverviewResponse> GetAllAssetCurrentPosition()
        {
            var currentPositions = _unitOfWork.PositionRepository.GetAll();
            var positionOverviewList = new List<PositionAssetOverviewResponse>();

            foreach (var position in currentPositions)
            {
                var overview = GeneratePositionOverview(position);
                positionOverviewList.Add(overview);
            }

            return positionOverviewList;
        }

        public IEnumerable<AssetTypeOverviewResponse> GetOverview()
        {
            return _unitOfWork.PositionRepository.GetAll()
                .GroupBy(x => new { x.Asset.Type, x.Asset.Currency })
                .Select(x => new AssetTypeOverviewResponse
                {
                    AssetType = x.FirstOrDefault()!.Asset.Type.ToString(),
                    Value = x.Sum(p => p.Quantity * p.Asset.Price),
                    Currency = x.FirstOrDefault()!.Asset.Currency.ToString()
                });
        }

        public IEnumerable<PositionBalanceResponse> GetBalanceOverall()
        {
            var currentPositions = _unitOfWork.PositionRepository.GetAll();
            return GeneratePositionBalanceByCurrency(currentPositions);
        }

        public Position CreateOrUpdatePosition(Asset asset, Transaction transaction)
        {
            var position = _unitOfWork.PositionRepository.GetPositionByAssetTicket(asset.Ticker).GetAwaiter().GetResult();

            if (position is null)
                return CreateAssetPosition(asset, transaction.Quantity, transaction.Price);

            return UpdateAssetPosition(position, transaction);
        }

        private Position UpdateAssetPosition(Position position, Transaction transaction)
        {
            try
            {
                if (transaction.OperatingType == OperationType.Buy)
                    return UpdateBuyStockHolder(transaction.Quantity, transaction.Price, position);

                if (transaction.OperatingType == OperationType.Sell)
                    return UpdateSellStockHolder(transaction.Quantity, position);

                throw new ArgumentException($"Invalid operation type: {transaction.OperatingType}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update the position for asset {position.Asset.Ticker}: {ex.Message}", ex);
            }
        }

        private Position UpdateBuyStockHolder(decimal buyQuantity, decimal buyPrice, Position position)
        {
            try
            {
                decimal averagePrice = CalculateAveragePrice(position, buyPrice, buyQuantity);

                position.AveragePrice = averagePrice;
                position.Quantity += buyQuantity;

                _unitOfWork.PositionRepository.Update(position);

                return position;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update StockHolder: {ex.Message}", ex);
            }
        }

        private Position UpdateSellStockHolder(decimal sellQuantity, Position position)
        {
            try
            {
                if (position.Quantity < sellQuantity)
                    throw new ArgumentException($"Quantity informed is bigger than avaiable. Quantity: {sellQuantity} Available: {position.Quantity}");

                position.Quantity -= sellQuantity;

                _unitOfWork.PositionRepository.Update(position);

                return position;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update StockHolder: {ex.Message}", ex);
            }
        }

        private Position CreateAssetPosition(Asset asset, decimal quantity, decimal price)
        {
            try
            {
                var position = new Position
                {
                    AveragePrice = price,
                    Quantity = quantity,
                    Asset = asset
                };

                _unitOfWork.PositionRepository.Create(position);

                return position;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create a new position for asset {asset.Ticker}: {ex.Message}", ex);
            }
        }

        private static List<PositionBalanceResponse> GeneratePositionBalanceByCurrency(IEnumerable<Position> positions)
        {
            var balanceGroupBy = new List<PositionBalanceResponse>();

            var groupedByCurrency = positions.GroupBy(p => p.Asset.Currency);

            foreach (var currency in groupedByCurrency)
            {
                var balance = GeneratePositionBalance(currency, currency.Key);
                balanceGroupBy.Add(balance);
            }

            return balanceGroupBy;
        }

        private static PositionBalanceResponse GeneratePositionBalance(IEnumerable<Position> positions, Currency currency)
        {
            var balance = new PositionBalanceResponse();

            foreach (var position in positions)
            {
                var overview = GeneratePositionOverview(position);
                balance.TotalInvested += overview.TotalInvested;
                balance.CurrentInvested += overview.CurrentInvested;
                balance.Currency = currency.ToString();
            }

            balance.TotalGain = balance.CurrentInvested - balance.TotalInvested;
            balance.GainPercentage = balance.TotalInvested != 0 ? (balance.TotalGain / balance.TotalInvested) * 100 : 0;

            return balance;
        }

        private static PositionAssetOverviewResponse GeneratePositionOverview(Position position)
        {
            return new PositionAssetOverviewResponse
            {
                Ticker = position.Asset.Ticker,
                AveragePrice = position.AveragePrice,
                Quantity = position.Quantity,
                TotalInvested = position.AveragePrice * position.Quantity,
                CurrentPrice = position.Asset.Price,
                CurrentInvested = position.Asset.Price * position.Quantity,
                CurrentGain = (position.Asset.Price * position.Quantity) - (position.AveragePrice * position.Quantity),
                GainPercentage = ((position.Asset.Price * position.Quantity) - (position.AveragePrice * position.Quantity)) / (position.AveragePrice * position.Quantity) * 100,
                AssetType = position.Asset.Type.ToString(),
                Currency = position.Asset.Currency.ToString()
            };
        }

        private static decimal CalculateAveragePrice(Position position, decimal buyPrice, decimal buyQuantity)
        {
            try
            {
                decimal currentPrice = position.AveragePrice * position.Quantity;
                decimal totalQuantity = buyQuantity + position.Quantity;

                decimal newAveragePrice = (currentPrice + (buyPrice * buyQuantity)) / totalQuantity;

                return newAveragePrice;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed while calculating the new average price: {ex.Message}", ex);
            }
        }
    }
}
