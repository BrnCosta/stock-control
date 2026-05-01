using Microsoft.EntityFrameworkCore;
using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;

namespace StockControl.Infrastructure.Repositories
{
  public class TradeRepository(AppDbContext context) : BaseRepository<Trade>(context), ITradeRepository
  {
  }
}
