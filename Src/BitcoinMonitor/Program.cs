using BitcoinMonitor.BackgroundServices;
using BitcoinMonitor.Data;
using BitcoinMonitor.Domain.Interfaces.CurrenciesExchange;
using BitcoinMonitor.Domain.Models.Configuration;
using BitcoinMonitor.Hubs;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Infrastructure.CoinbaseExchangeProvider;
using Infrastructure.CoinbaseExchangeProvider.AutoMapperProfile;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

//Logger configuration
var appInsightKey = builder.Configuration.GetValue<string>("appInsightKey");

//Add Serilog for logging instead of default logger
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    if (string.IsNullOrEmpty(appInsightKey))
        loggerConfiguration.WriteTo.Console();
    else
        loggerConfiguration.WriteTo.ApplicationInsights(appInsightKey, new TraceTelemetryConverter());
});

//Adding telemery
if (string.IsNullOrEmpty(appInsightKey))
    builder.Services.AddApplicationInsightsTelemetry(o =>
    {
        o.InstrumentationKey = appInsightKey;
        o.EnableAdaptiveSampling = false;
    });

//Add automapper profiles
builder.Services.AddAutoMapper(typeof(CoinbaseAutoMapperProfile).Assembly);

//Get exchange rate configuration and provider
var exchangeRateProviderConfig = builder.Configuration.GetSection("ExchangeRateProviderConfiguration")
    .Get<ExchangeRateProviderConfiguration>();
builder.Services.AddSingleton(exchangeRateProviderConfig);
builder.Services.AddScoped<IExchangeRateProvider, CoinbaseExchangeRateProvider>();

bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
//Adding database context
builder.Services.AddDbContext<BitcoinMonitorContext>(
                options =>
                {
                    //For running the application on localhost the in memory database is used
                    if(!useInMemoryDatabase)
                        options.UseSqlServer(builder.Configuration.GetConnectionString("MsSql"));
                    else
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
