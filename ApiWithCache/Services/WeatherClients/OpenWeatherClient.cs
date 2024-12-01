using ApiWithCache.Options;
using ApiWithCache.Services.WeatherClients.Models;
using Microsoft.Extensions.Options;

namespace ApiWithCache.Services.WeatherClients
{
    public interface IWeatherClient
    {
        Task<WeatherResponse?> GetCurrentWeatherByCoordinates(double longitude, double latitude);
    }

    public class OpenWeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<OpenWeatherApiOptions> _options;

        public OpenWeatherClient(IHttpClientFactory httpClientFactory, IOptions<OpenWeatherApiOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient("weatherapi");
            _options = options;
        }

        public async Task<WeatherResponse?> GetCurrentWeatherByCoordinates(double longitude, double latitude)
        {
            var requestUri = $"weather?lat={latitude}&lon={longitude}&appid={_options.Value.ApiKey}&units=metric";
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch weather data from the external API.");
            }

            return await response.Content.ReadFromJsonAsync<WeatherResponse>();
        }
    }
}
