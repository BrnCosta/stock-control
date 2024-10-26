using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockControl.Application.Services
{
  public class StockPriceUpdateBgService(ILogger<StockPriceUpdateBgService> logger, IHttpClientFactory httpClient, IUnitOfWork unitOfWork) : BackgroundService
  {
    private class StockResult
    {
      public double RegularMarketPrice { get; set; }
    }

    private class BrApiResponse
    {
      public required List<StockResult> Results { get; set; }
    }

    private const string BRAPI_QUOTE_URL = "https://brapi.dev/api/quote/";
    private const string BRAPI_INTERVAL_PARAMS = "?range=1d&interval=1d";

    private readonly ILogger<StockPriceUpdateBgService> _logger = logger;
    private readonly IHttpClientFactory _httpClient = httpClient;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

        await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
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

      if (currentTime.Hour >= 10 && currentTime.Hour <= 19)
        return false;

      return true;
    }

    private async void PerformStockUpdate(CancellationToken stoppingToken)
    {
      try
      {
        var httpClient = _httpClient.CreateClient();

        _logger.LogInformation("Performing API call to update stock prices...");

        var stockHoldersSymbols = GetAllStockHoldersSymbols();

        foreach(var symbol in stockHoldersSymbols)
        {
          double actualPrice = GetStockActualPrice(symbol, stoppingToken).GetAwaiter().GetResult();

          if (actualPrice == 0.0)
            continue;

          await UpdateStockPrice(symbol, actualPrice);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to update stock prices. Trying again in 30 minutes...");
      }
    }

    private async Task<double> GetStockActualPrice(string symbol, CancellationToken stoppingToken)
    {
      double actualPrice = 0.0;

      try
      {
        string apiUrl = String.Concat(BRAPI_QUOTE_URL, symbol, BRAPI_INTERVAL_PARAMS);

        var httpClient = _httpClient.CreateClient();
        _logger.LogInformation("Getting actual price value for ${symbol}", symbol);

        var response = (await httpClient.GetAsync(apiUrl, stoppingToken)).EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadFromJsonAsync<BrApiResponse>(stoppingToken);

        actualPrice = jsonResponse?.Results.FirstOrDefault()?.RegularMarketPrice ?? throw new Exception("Cannot retrieve stock actual price.");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while getting actual price from ${symbol}", symbol);
      }

      return actualPrice;
    }

    private List<string> GetAllStockHoldersSymbols()
    {
      var stockHoldersSymbols = new List<string>();

      foreach(var stockHolder in _unitOfWork.StockHolderRepository.GetAll())
        stockHoldersSymbols.Add(stockHolder.StockSymbol);

      return stockHoldersSymbols;
    }

    private async Task UpdateStockPrice(string symbol, double actualPrice)
    {
      try
      {
        Stock stock = await _unitOfWork.StockRepository.GetAsync(symbol) ?? throw new Exception($"Failed to get {symbol} in database.");

        stock.Price = actualPrice;

        _unitOfWork.StockRepository.Update(stock);

        await _unitOfWork.Commit();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error while updating stock price ${symbol}", symbol);
      }
    }
  }
}
