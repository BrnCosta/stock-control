using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockControl.Application.Services;
using StockControl.Application.Services.External;
using StockControl.Core.Interfaces;
using StockControl.Core.Interfaces.Repositories;
using StockControl.Core.Interfaces.Services;
using StockControl.Core.Interfaces.Services.External;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositorioes
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IDividendRepository, DividendRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Services
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<ITradeService, TradeService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IDividendService, DividendService>();
            services.AddScoped<ITransactionService, TransactionService>();

            // External
            services.AddScoped<IAssetInformationService, BrApiInformationService>();
        }
    }
}
