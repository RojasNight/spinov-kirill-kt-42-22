using Microsoft.AspNetCore.Mvc;

namespace SpinovKirillKT_42_22.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }




        [HttpPost(Name = "AddNewSummary")]

        public string[] AddNewSummary(string newSummary)
        {
            _logger.LogError("Method was called");

            var list = Summaries.ToList();
            list.Add(newSummary);
            return list.ToArray();
        }
    }
}
