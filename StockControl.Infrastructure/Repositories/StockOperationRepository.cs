using StockControl.Core.Entities;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Infrastructure.Repositories
{
    public class StockOperationRepository(AppDbContext context) : 
        BaseRepository<StockOperation>(context), IStockOperationRepository
    {
    }
}
