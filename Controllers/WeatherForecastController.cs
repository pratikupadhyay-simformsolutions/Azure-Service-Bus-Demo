using Demo_Azure.DTO.Request;
using Demo_Azure.IServices;
using Demo_Azure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace Demo_Azure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly PratikServiceBus _pratikServiceBus;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, PratikServiceBus pratikServiceBus)
        {
            _logger = logger;
            _configuration = configuration;
            _pratikServiceBus = pratikServiceBus;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("azurefunction")]
        public async Task<StatusCodeResult> AzureFunction([FromBody] AzureJsonRequest azureJsonRequest)
        {
            await _pratikServiceBus.SendMessageAsync(azureJsonRequest);
            var serialize = JsonConvert.SerializeObject(azureJsonRequest);
            _logger.LogInformation("jsonRequest: ", serialize);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}