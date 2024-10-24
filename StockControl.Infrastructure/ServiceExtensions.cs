﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockControl.Application.Services;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Infrastructure.Context;
using StockControl.Infrastructure.Repositories;

namespace StockControl.Infrastructure
{
  public static class ServiceExtensions
  {
    public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<AppDbContext>(
          opt => opt.UseSqlite(
              configuration.GetConnectionString("Sqlite"),
              db => db.MigrationsAssembly("StockControl.API"))
      );

      // Repositorioes
      services.AddScoped<IStockRepository, StockRepository>();
      services.AddScoped<IStockHolderRepository, StockHolderRepository>();
      services.AddScoped<IStockOperationRepository, StockOperationRepository>();

      // Services
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped<IStockService, StockService>();
      services.AddScoped<IOperationService, OperationService>();
      services.AddScoped<IStockHolderService, StockHolderService>();
    }
  }
}
