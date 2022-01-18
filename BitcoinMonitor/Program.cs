using BitcoinMonitor.Data;
using Infrastructure.CoinbaseExchangeProvider;
using Infrastructure.CoinbaseExchangeProvider.AutoMapperProfile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

//Add automapper profiles
builder.Services.AddAutoMapper(typeof(CoinbaseAutoMapperProfile).Assembly);

//Add exchange rate provider
builder.Services.CoinbaseExchangeProvider("https://api.coinbase.com");

//Adding database context
builder.Services.AddDbContext<BitcoinMonitorContext>(
                options =>
                {
                    options.UseInMemoryDatabase("ApplicationDatabase");
                });

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();



builder.Services.AddScoped<ExchangeRateService>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddHostedService<BitcoinMonitor.BackgroundServices.EchangeRateMonitor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
