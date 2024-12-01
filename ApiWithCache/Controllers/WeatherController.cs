using ApiWithCache.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{cityName}")]
        public async Task<IActionResult> GetWeather(string cityName)
        {
            var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);

            return Ok(weather);
        }
    }
}
