using Microsoft.AspNetCore.Mvc;
using Polly_eg.Policies;

namespace Polly_eg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       
     
        private readonly ClientPolicy _clientPolicy;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(ClientPolicy clientPolicy,
               IHttpClientFactory httpClientFactory)
        {
           
            _clientPolicy = clientPolicy;
            _httpClientFactory = httpClientFactory;
        }

       

        [HttpGet(Name = "PollyRequest")]
        public async Task<ActionResult> Get()
        {
            //var client = new HttpClient();

            var client =  _httpClientFactory.CreateClient();
            // var response = await client.GetAsync("https://localhost:7096/WeatherForecast/Number/100");

            //-- immediate http retry
            //var response = await _clientPolicy.ImmediateHttpRetry.ExecuteAsync(
            //    () => client.GetAsync("https://localhost:7096/WeatherForecast/Number/100"));

           // --linear http retry
             var response = await _clientPolicy.LinearHttpRetry.ExecuteAsync(
               () => client.GetAsync("https://localhost:7096/WeatherForecast/Number/100"));

            // -- exponential http retry
            //var response = await _clientPolicy.ExponentialHttpRetry.ExecuteAsync(
            // () => client.GetAsync("https://localhost:7096/WeatherForecast/Number/100"));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("-----SERVER Response: SUCCESS ---- ");
                return Ok();
            }
            Console.WriteLine("-----SERVER Response: FAILURE ---- ");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}