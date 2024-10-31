using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services.External;

namespace StockControl.Application.Services
{
  public class StockPriceUpdateBackgroundService(ILogger<StockPriceUpdateBackgroundService> logger, IServiceScopeFactory serviceScope) : BackgroundService
  {
    private const int UPDATE_TIME_MINUTES = 30;

    private readonly ILogger<StockPriceUpdateBackgroundService> _logger = logger;
    private readonly IServiceScopeFactory _serviceScope = serviceScope;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("Stock Price Update - Background Service is starting.");

      while (!stoppingToken.IsCancellationRequested)
      {
        if (IsWithinBusinessHours(DateTime.Now))
        {
          PerformStockUpdate(stoppingToken);
        }
        else
        {
          _logger.LogWarning("Stock market is not open, waiting until commercial time.");
        }

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

    private async void PerformStockUpdate(CancellationToken stoppingToken)
    {
      try
      {
        _logger.LogInformation("Performing API call to update stock prices...");

        using var internScope = _serviceScope.CreateScope();

        var unitOfWork = internScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var stockInformationService = internScope.ServiceProvider.GetRequiredService<IStockInformationService>();

        var stockHoldersSymbols = GetAllStockHoldersSymbols(unitOfWork);

        foreach (var symbol in stockHoldersSymbols)
        {
          double actualPrice = GetStockActualPrice(symbol, stockInformationService).GetAwaiter().GetResult();

          if (actualPrice == 0.0)
            continue;

          await UpdateStockPrice(symbol, actualPrice, unitOfWork);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to update stock prices. Trying again in ${time} minutes...", UPDATE_TIME_MINUTES);
      }
    }

    private async Task<double> GetStockActualPrice(string symbol, IStockInformationService stockInformationService)
    {
      try
      {
        _logger.LogInformation("Getting actual price value for ${symbol}", symbol);

        StockInformationResult stockInformation = await stockInformationService.GetStockInformation(symbol);

        return stockInformation.RegularMarketPrice;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while getting actual price from ${symbol}", symbol);
      }

      return 0.0;
    }

    private static List<string> GetAllStockHoldersSymbols(IUnitOfWork unitOfWork)
    {
      var stockHoldersSymbols = new List<string>();

      foreach(var stockHolder in unitOfWork.StockHolderRepository.GetAll())
        stockHoldersSymbols.Add(stockHolder.StockSymbol);

      return stockHoldersSymbols;
    }

    private async Task UpdateStockPrice(string symbol, double actualPrice, IUnitOfWork unitOfWork)
    {
      try
      {
        Stock stock = await unitOfWork.StockRepository.GetAsync(symbol) ?? throw new Exception($"Failed to get {symbol} in database.");

        stock.Price = actualPrice;

        unitOfWork.StockRepository.Update(stock);

        await unitOfWork.Commit();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while updating stock price ${symbol}", symbol);
      }
    }
  }
}
