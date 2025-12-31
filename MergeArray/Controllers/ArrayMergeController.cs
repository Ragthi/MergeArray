using Microsoft.AspNetCore.Mvc;

namespace MergeArray.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArrayMergeController : ControllerBase
    {
        private readonly ILogger<ArrayMergeController> _logger;

        public ArrayMergeController(ILogger<ArrayMergeController> logger)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
