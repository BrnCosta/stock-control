using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Formats.Asn1.AsnWriter;

namespace StockControl.Application.Services
{
  public class StockPriceUpdateBackgroundService(ILogger<StockPriceUpdateBackgroundService> logger,
    IHttpClientFactory httpClient, IServiceScopeFactory serviceScope) : BackgroundService
  {
    private const string BRAPI_QUOTE_URL = "https://brapi.dev/api/quote/";
    private const string BRAPI_INTERVAL_PARAMS = "?range=1d&interval=1d";
    private const int UPDATE_TIME_MINUTES = 30;
    private const string AUTHENTICATION_TOKEN = "13txL2WCFqDiS9eGn9gUQF";

    private readonly ILogger<StockPriceUpdateBackgroundService> _logger = logger;
    private readonly IHttpClientFactory _httpClient = httpClient;

    private readonly IServiceScopeFactory _serviceScope = serviceScope;

    private class StockResult
    {
      public double RegularMarketPrice { get; set; }
    }

    private class BrApiResponse
    {
      public required List<StockResult> Results { get; set; }
    }

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
        var httpClient = _httpClient.CreateClient();

        _logger.LogInformation("Performing API call to update stock prices...");

        using var internScope = _serviceScope.CreateScope();

        var unitOfWork = internScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var stockHoldersSymbols = GetAllStockHoldersSymbols(unitOfWork);

        foreach (var symbol in stockHoldersSymbols)
        {
          double actualPrice = GetStockActualPrice(symbol, stoppingToken).GetAwaiter().GetResult();

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

    private async Task<double> GetStockActualPrice(string symbol, CancellationToken stoppingToken)
    {
      double actualPrice = 0.0;

      try
      {
        string apiUrl = String.Concat(BRAPI_QUOTE_URL, symbol, BRAPI_INTERVAL_PARAMS);

        var httpClient = _httpClient.CreateClient();
        _logger.LogInformation("Getting actual price value for ${symbol}", symbol);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AUTHENTICATION_TOKEN);

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
