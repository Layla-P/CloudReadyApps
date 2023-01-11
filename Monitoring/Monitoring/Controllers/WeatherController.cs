using Microsoft.AspNetCore.Mvc;

namespace Monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            _logger.LogInformation("the GET /weather endpoint was just triggered");


            var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

            var forecast = Enumerable.Range(1, 5).Select(index =>
         new WeatherForecast
         (
             DateTime.Now.AddDays(index),
             Random.Shared.Next(-20, 55),
             summaries[Random.Shared.Next(summaries.Length)]
         ))
         .ToArray();
            return new JsonResult(forecast);
        }

    }
}
