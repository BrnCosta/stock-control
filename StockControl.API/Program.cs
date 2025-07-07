using StockControl.API;
using StockControl.Application.Services;
using StockControl.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigurePersistenceApp(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.AddHostedService<StockPriceUpdateBackgroundService>();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

var frontendCorsPolicy = "_frontendOrigin";

builder.Services.AddCors(options =>
{
  options.AddPolicy(name: frontendCorsPolicy,
    policy =>
    {
      policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

builder.Logging.AddConsole();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

  // Load Environment Variables from .env file
  DotEnv.Load();

  app.UseSwagger();
  app.UseSwaggerUI();

  app.UseCors(frontendCorsPolicy);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
