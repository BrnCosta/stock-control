using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;

namespace StockControl.Application.Services
{
  public class DividendService(IUnitOfWork unitOfWork) : IDividendService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public void CreateNewDividend(string stockSymbol, double value, DateOnly date)
    {
      var dividend = new Dividend()
      {
        StockSymbol = stockSymbol,
        Value = value,
        Date = date
      };

      _unitOfWork.DividendRepository.Create(dividend);
    }
  }
}
