﻿using StockControl.Core.Entities;
using StockControl.Core.Enums;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Services;

namespace StockControl.Application.Services
{
  public class OperationService(IUnitOfWork unitOfWork) : IOperationService
  {
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public void CreateNewOperation(Stock stock, double buyPrice, int quantity)
    {
      var stockOperation = new StockOperation
      {
        Date = DateTime.Now,
        OperatingType = OperationType.Buy,
        Quantity = quantity,
        Price = buyPrice,
        StockSymbol = stock.Symbol,
      };

      _unitOfWork.OperationRepository.Create(stockOperation);
    }
  }
}