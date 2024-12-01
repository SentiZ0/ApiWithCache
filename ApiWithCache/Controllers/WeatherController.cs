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

        [HttpGet("{latitude}/{longitude}")]
        public async Task<IActionResult> GetWeather(double latitude, double longitude)
        {
            var weather = await _weatherService.GetWeatherAsync(latitude, longitude);

            return Ok(weather);
        }
    }
}
