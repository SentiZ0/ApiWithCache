using ApiWithCache.Services.WeatherClients;
using ApiWithCache.Services.WeatherClients.Models;

namespace ApiWithCache.Services
{
    public interface IWeatherService
    {
        Task<Models.Weather> GetWeatherByCityNameAsync(string name);
        Task ClearCache(string cityName);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IWeatherClient _weatherClient;
        private readonly ICacheService _cacheService;

        public WeatherService(IWeatherClient weatherClient, ICacheService cacheService)
        {
            _weatherClient = weatherClient;
            _cacheService = cacheService;
        }

        public async Task<Models.Weather> GetWeatherByCityNameAsync(string name)
        {
            var cacheValue = await _cacheService.GetAsync<WeatherResponse>(name);

            if (cacheValue is null)
            {
                var response = await _weatherClient.GetCurrentWeatherByCity(name);

                if (response is null)
                {
                    throw new Exception("Weather data not found");
                }

                await _cacheService.SetAsync(name, response);

                cacheValue = response;
            }

            return new Models.Weather
            {
                Name = cacheValue.Name,
                Longitude = cacheValue.Coord.Lon,
                Latitude = cacheValue.Coord.Lat,
                Description = cacheValue.Weather[0].Description,
                Country = cacheValue.Sys.Country,
                TempMin = cacheValue.Main.Temp_Min,
                TempMax = cacheValue.Main.Temp_Max,
                Temp = cacheValue.Main.Temp,
                FeelsLike = cacheValue.Main.Feels_Like,
                Pressure = cacheValue.Main.Pressure,
                Humidity = cacheValue.Main.Humidity,
                WindSpeed = cacheValue.Wind.Speed,
                Sunrise = cacheValue.Sys.Sunrise,
                Sunset = cacheValue.Sys.Sunset
            };
        }

        public async Task ClearCache(string cityName)
        {
            await _cacheService.RemoveAsync(cityName);
        }
    }
}
