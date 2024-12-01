using ApiWithCache.Services.WeatherClients;
using ApiWithCache.Services.WeatherClients.Models;

namespace ApiWithCache.Services
{
    public interface IWeatherService
    {
        Task<Models.Weather> GetWeatherAsync(double latitude, double longitude);
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

        public async Task<Models.Weather> GetWeatherAsync(double latitude, double longitude)
        {
            var response = await _weatherClient.GetCurrentWeatherByCoordinates(longitude, latitude);

            if (response is null)
            {
                throw new Exception("Weather data not found");
            }

            if (string.IsNullOrEmpty(response.Name))
            {
                throw new InvalidOperationException("Response does not contain a valid name for caching.");
            }

            var cacheValue = _cacheService.Get<WeatherResponse>(response.Name) ?? response;

            if (cacheValue == response)
            {
                _cacheService.Set(response.Name, response);
            }

            return new Models.Weather
            {
                Longitude = longitude,
                Latitude = latitude,
                Name = cacheValue.Name,
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
    }
}
