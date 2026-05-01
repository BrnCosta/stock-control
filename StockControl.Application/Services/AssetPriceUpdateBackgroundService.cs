using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services.External;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StockControl.Application.Services
{
    public class AssetPriceUpdateBackgroundService(ILogger<AssetPriceUpdateBackgroundService> logger,
      IHttpClientFactory httpClient, IServiceScopeFactory serviceScope) : BackgroundService
    {
        private const int UPDATE_TIME_MINUTES = 30;
        private const string AUTHENTICATION_TOKEN = "13txL2WCFqDiS9eGn9gUQF";

        private readonly ILogger<AssetPriceUpdateBackgroundService> _logger = logger;
        private readonly IHttpClientFactory _httpClient = httpClient;

        private readonly IServiceScopeFactory _serviceScope = serviceScope;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stock Price Update - Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (IsWithinBusinessHours(DateTime.Now))
                {
                    await PerformStockUpdate();
                    return;
                }

                _logger.LogWarning("Stock market is not open, waiting until commercial time.");

                await Task.Delay(TimeSpan.FromMinutes(UPDATE_TIME_MINUTES), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stock Price Update - Background Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        private static bool IsWithinBusinessHours(DateTime currentTime)
        {
            if (currentTime.DayOfWeek == DayOfWeek.Saturday || currentTime.DayOfWeek == DayOfWeek.Sunday)
                return false;

            if (currentTime.Hour <= 10 || currentTime.Hour >= 19)
                return false;

            return true;
        }

        private async Task PerformStockUpdate()
        {
            try
            {
                var httpClient = _httpClient.CreateClient();

                _logger.LogInformation("Performing API call to update stock prices...");

                using var internalScope = _serviceScope.CreateScope();

                var unitOfWork = internalScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var assetInformationService = internalScope.ServiceProvider.GetRequiredService<IAssetInformationService>();

                var assets = GetAllCurrentAssetPositions(unitOfWork);

                foreach (var asset in assets)
                {
                    decimal currentPrice = await assetInformationService.GetAssetMarketPrice(asset.Ticker);

                    if (currentPrice == 0.0m)
                        continue;

                    await UpdateStockPrice(asset, currentPrice, unitOfWork);
                }

                await unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update stock prices. Trying again in ${time} minutes...", UPDATE_TIME_MINUTES);
            }
        }

        private static List<Asset> GetAllCurrentAssetPositions(IUnitOfWork unitOfWork)
        {
            return unitOfWork.PositionRepository.GetAll().Select(p => p.Asset).ToList();
        }

        private async Task UpdateStockPrice(Asset asset, decimal currentPrice, IUnitOfWork unitOfWork)
        {
            try
            {
                asset.Price = currentPrice;
                unitOfWork.AssetRepository.Update(asset);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating stock price ${symbol}", asset);
            }
        }
    }
}
