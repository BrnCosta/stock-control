using Moq;
using StockControl.Application.Services;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Requests;

namespace StockControl.UnitTest.Services
{
    [TestClass]
    public sealed class TradeServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPositionService> _positionServiceMock = new();
        private readonly Mock<IAssetService> _assetServiceMock = new();
        private readonly Mock<ITransactionService> _transactionServiceMock = new();

        [TestMethod]
        public void Test_CreateNewTrade_WithNoTransactions_ShouldThrowArgumentException()
        {
            var tradeRequest = new TradeRequest
            {
                Tax = 10,
                Date = DateTime.Now,
                Transactions = [],
                Currency = "CAD"
            };

            var tradeService = new TradeService(_unitOfWorkMock.Object, _positionServiceMock.Object, _assetServiceMock.Object, _transactionServiceMock.Object);

            Assert.ThrowsAsync<ArgumentException>(() => tradeService.CreateNewTrade(tradeRequest));
        }

        [TestMethod]
        public void Test_CreateNewTrade_WithEmptyTicker_ShouldThrowArgumentException()
        {
            var tradeRequest = new TradeRequest
            {
                Tax = 10,
                Date = DateTime.Now,
                Currency = "CAD",
                Transactions = [new TransactionRequest
                {
                    Ticker = "",
                    Quantity = 100,
                    Price = 50,
                    OperatingType = OperationType.Buy.ToString()
                }]
            };

            var tradeService = new TradeService(_unitOfWorkMock.Object, _positionServiceMock.Object, _assetServiceMock.Object, _transactionServiceMock.Object);

            Assert.ThrowsAsync<ArgumentException>(() => tradeService.CreateNewTrade(tradeRequest));
        }

        [TestMethod]
        public void Test_CreateNewTrade_WithEmptyCurrency_ShouldThrowArgumentException()
        {
            var tradeRequest = new TradeRequest
            {
                Tax = 10,
                Date = DateTime.Now,
                Currency = "",
                Transactions = [new TransactionRequest
                {
                    Ticker = "AAPL",
                    Quantity = 100,
                    Price = 50,
                    OperatingType = OperationType.Buy.ToString()
                }]
            };

            var tradeService = new TradeService(_unitOfWorkMock.Object, _positionServiceMock.Object, _assetServiceMock.Object, _transactionServiceMock.Object);

            Assert.ThrowsAsync<ArgumentException>(() => tradeService.CreateNewTrade(tradeRequest));
        }

        [TestMethod]
        public void Test_CreateNewTrade_WithInvalidCurrency_ShouldThrowArgumentException()
        {
            var tradeRequest = new TradeRequest
            {
                Tax = 10,
                Date = DateTime.Now,
                Currency = "EUR",
                Transactions = [new TransactionRequest
                {
                    Ticker = "AAPL",
                    Quantity = 100,
                    Price = 50,
                    OperatingType = OperationType.Buy.ToString()
                }]
            };

            var tradeService = new TradeService(_unitOfWorkMock.Object, _positionServiceMock.Object, _assetServiceMock.Object, _transactionServiceMock.Object);

            Assert.ThrowsAsync<ArgumentException>(() => tradeService.CreateNewTrade(tradeRequest));
        }

        [TestMethod]
        public void Test_CreateNewTrade_OneTransaction_ShouldReturnValidGuid()
        {
            var tradeRepoMock = new Mock<ITradeRepository>();

            _unitOfWorkMock.Setup(x => x.TradeRepository).Returns(tradeRepoMock.Object);

            var tradeRequest = new TradeRequest
            {
                Tax = 10,
                Date = DateTime.Now,
                Currency = "CAD",
                Transactions = [new TransactionRequest
                {
                    Ticker = "AAPL",
                    Quantity = 100,
                    Price = 50,
                    OperatingType = OperationType.Buy.ToString()
                }]
            };

            var tradeService = new TradeService(_unitOfWorkMock.Object, _positionServiceMock.Object, _assetServiceMock.Object, _transactionServiceMock.Object);

            var validGuid = tradeService.CreateNewTrade(tradeRequest);

            Assert.IsNotNull(validGuid);
            Assert.IsTrue(Guid.TryParse(validGuid.Result.ToString(), out var _));
        }
    }
}
