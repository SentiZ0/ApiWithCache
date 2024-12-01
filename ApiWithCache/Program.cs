using ApiWithCache.Services.WeatherClients;
using ApiWithCache.Services;
using ApiWithCache.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.Configure<OpenWeatherApiOptions>(
    builder.Configuration.GetSection("OpenWeatherApi"));

builder.Services.AddHttpClient("weatherapi", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<OpenWeatherApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
