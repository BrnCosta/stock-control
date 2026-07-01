using Moq;
using StockControl.Application.Services;
using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;

namespace StockControl.UnitTest.Services
{
    [TestClass]
    public sealed class PositionServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        [TestMethod]
        public async Task Test_GetOverview_ShouldReturnEmptyList()
        {
            var service = new PositionService(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.PositionRepository.GetAll()).Returns([]);
            var result = service.GetOverview();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void Test_GetOverview_ShouldReturnCorrectTotalValue()
        {
            var positions = new List<Position>
            {
                new()
                {
                    Asset = new Asset { Ticker = "APPL3", Type = AssetType.STOCK, Currency = Currency.BRL, Price = 100 },
                    Quantity = 10
                },
                new()
                {
                    Asset = new Asset { Ticker = "APPL4", Type = AssetType.STOCK, Currency = Currency.BRL, Price = 100 },
                    Quantity = 10
                },
                new()
                {
                    Asset = new Asset { Ticker = "APPL", Type = AssetType.ETF, Currency = Currency.CAD, Price = 50 },
                    Quantity = 100
                }
            };

            _unitOfWorkMock.Setup(u => u.PositionRepository.GetAll()).Returns(positions);

            var service = new PositionService(_unitOfWorkMock.Object);

            var result = service.GetOverview();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(2000, result.FirstOrDefault(r => r.AssetType == AssetType.STOCK.ToString())!.Value);
            Assert.AreEqual(5000, result.FirstOrDefault(r => r.AssetType == AssetType.ETF.ToString())!.Value);
        }

        [TestMethod]
        public void Test_GetBalanceOverall_NoPosition_ShouldReturnEmptyList()
        {
            var service = new PositionService(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.PositionRepository.GetAll()).Returns([]);
            var result = service.GetBalanceOverall();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void Test_GetBalanceOverall_SinglePosition_ComputesValuesCorrectly()
        {
            var position = new Position 
            { 
                Asset = new Asset { Ticker = "ABC", Type = AssetType.STOCK, Currency = Currency.BRL, Price = 100m }, 
                AveragePrice = 80m, 
                Quantity = 10 
            };

            _unitOfWorkMock.Setup(u => u.PositionRepository.GetAll()).Returns([position]);

            var service = new PositionService(_unitOfWorkMock.Object);

            var result = service.GetBalanceOverall();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            var balance = result.First();
            Assert.AreEqual(800m, balance.TotalInvested);
            Assert.AreEqual(1000m, balance.CurrentInvested);
            Assert.AreEqual(200m, balance.TotalGain);
            Assert.AreEqual(25m, balance.GainPercentage);
        }

        [TestMethod]
        public void Test_GetBalanceOverall_MultiplePositions_ComputesValuesCorrectly()
        {
            var positions = new List<Position>
            {
                new()
                {
                    Asset = new Asset { Ticker = "APPL3", Type = AssetType.STOCK, Currency = Currency.BRL, Price = 100 },
                    Quantity = 10,
                    AveragePrice = 200
                },
                new()
                {
                    Asset = new Asset { Ticker = "APPL4", Type = AssetType.STOCK, Currency = Currency.BRL, Price = 100 },
                    Quantity = 10,
                    AveragePrice = 200
                },
                new()
                {
                    Asset = new Asset { Ticker = "APPL", Type = AssetType.ETF, Currency = Currency.CAD, Price = 50 },
                    Quantity = 100,
                    AveragePrice = 25
                }
            };

            _unitOfWorkMock.Setup(u => u.PositionRepository.GetAll()).Returns(positions);

            var service = new PositionService(_unitOfWorkMock.Object);

            var result = service.GetBalanceOverall();
            Assert.AreEqual(2, result.Count());

            var balanceBrl = result.FirstOrDefault(r => r.Currency == Currency.BRL.ToString());
            Assert.AreEqual(4000, balanceBrl!.TotalInvested);
            Assert.AreEqual(2000m, balanceBrl.CurrentInvested);
            Assert.AreEqual(-2000m, balanceBrl.TotalGain);
            Assert.AreEqual(-50m, balanceBrl.GainPercentage);

            var balanceCad = result.FirstOrDefault(r => r.Currency == Currency.CAD.ToString());
            Assert.AreEqual(2500, balanceCad!.TotalInvested);
            Assert.AreEqual(5000m, balanceCad.CurrentInvested);
            Assert.AreEqual(2500m, balanceCad.TotalGain);
            Assert.AreEqual(100m, balanceCad.GainPercentage);
        }
    }
}
