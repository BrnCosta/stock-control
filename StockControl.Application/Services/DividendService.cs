using StockControl.Core.Entities;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Responses;

namespace StockControl.Application.Services
{
  public class DividendService(IUnitOfWork unitOfWork) : IDividendService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<Dividend> GetAll()
    {
      return _unitOfWork.DividendRepository.GetAll().ToList();
    }

    public List<DividendGroupByMonthResponse> GetGroupByMonth()
    {
      return _unitOfWork.DividendRepository.GetDividendsGroupedByMonth().ToList();
    }

    public List<DividendGroupByTickerResponse> GetGroupByTicker()
    {
      return _unitOfWork.DividendRepository.GetDividendsGroupedBySymbol().ToList();
    }
  }
}
