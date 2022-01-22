using BitcoinMonitor.BackgroundServices;
using BitcoinMonitor.Data;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Hubs;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Infrastructure.CoinbaseExchangeProvider;
using Infrastructure.CoinbaseExchangeProvider.AutoMapperProfile;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Add Serilog for logging instead of default logger
builder.Host.UseSerilog((context, loggerConfiguration) => 
    loggerConfiguration.WriteTo.Console());

//Add automapper profiles
builder.Services.AddAutoMapper(typeof(CoinbaseAutoMapperProfile).Assembly);

//Add exchange rate provider
builder.Services.AddScoped<IExchangeRateProvider, CoinbaseExchangeRateProvider>();

//Adding database context
builder.Services.AddDbContext<BitcoinMonitorContext>(
                options =>
                {
                    options.UseInMemoryDatabase("ApplicationDatabase");
                });

builder.Services.AddBlazorise(options =>
              {
                  options.ChangeTextOnKeyPress = true;
              })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


builder.Services.AddScoped<ExchangeRateService>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ExchangeRateMonitor>();
builder.Services.AddHostedService<ExchangeRateMonitor>((s) => s.GetRequiredService<ExchangeRateMonitor>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ExchangeRateUpdateHub>("/exchangerateupdatehub");
app.MapFallbackToPage("/_Host");

app.Run();
