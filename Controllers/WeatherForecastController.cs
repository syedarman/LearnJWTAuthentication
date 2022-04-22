using LearnJWTAuthentication.Models;
using LearnJWTAuthentication.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnJWTAuthentication.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AppSettings _appsettings;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptions<AppSettings> appsettings)
    {
        _logger = logger;
        _appsettings = appsettings.Value;
    }

    [Authorize]
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

    [Authorize]
    [HttpGet(Name = "GetConfigValues")]
    public string GetConfigValues()
    {
        return _appsettings.JWTSecret;
    }
}
