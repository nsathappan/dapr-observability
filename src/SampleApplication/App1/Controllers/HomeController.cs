using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Microsoft.AspNetCore.Mvc;

namespace App1.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong from App1");
        }

        [HttpGet("Secrets/{key}")]
        public async Task<IActionResult> GetSecret([FromServices] DaprClient daprClient, string key)
        {
            var secrets = await daprClient.GetSecretAsync("local-secrets-store", key);
            return Ok(secrets.First().Value ?? "Key not found");

        }

        [HttpPost("State/{key}/{value}")]
        public async Task<IActionResult> SetCache([FromServices] DaprClient daprClient, string key, string value)
        {
            await daprClient.SaveStateAsync("in-memory-state-store", key, value);
            return Ok($"Saved key {key} with value {value}");
        }

        [HttpGet("State/{key}")]
        public async Task<IActionResult> GetCache([FromServices] DaprClient daprClient, string key)
        {
            var value = await daprClient.GetStateAsync<string>("in-memory-state-store", key);
            return Ok(value ?? "Key not found");
        }

        [HttpGet("pingapp2")]
        public async Task<IActionResult> PingApp2([FromServices] DaprClient daprClient)
        {
            logger.LogInformation("Ping App2");
            string message;
            var client = DaprClient.CreateInvokeHttpClient("app2-Service");
            var response = await client.GetAsync("/ping");
            //var response = await daprClient.InvokeMethodAsync<HttpResponseMessage>(HttpMethod.Get, "app2-Service", "/ping");
            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
                logger.LogInformation("App2 Status: {message}", message);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                logger.LogError("Error calling App2  service: {errorResponse}", errorResponse);
                return StatusCode(500, $"Error calling App2 service: {errorResponse}");
            }
            return Ok(message);
        }

        [HttpGet("app2weatherforecast")]
        public async Task<IActionResult> PingApp2WeatherForecast([FromServices] DaprClient daprClient)
        {
            logger.LogInformation("Ping App2 WeatherForecast");
            var response = await daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get, "app2-Service", "/WeatherForecast");
            return Ok(response);
        }

        [HttpGet("pingapp2usinginvoke")]
        public async Task<IActionResult> PingApp2InvokeMethod([FromServices] DaprClient daprClient)
        {
            logger.LogInformation("Ping App2");
            string message;
            var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "app2-Service", "/ping");
            request.Headers.Add("Accept", "text/plain"); // Specify the Accept header

            var response = await daprClient.InvokeMethodWithResponseAsync(request);

            if (response.IsSuccessStatusCode)
            {
                message = await response.Content.ReadAsStringAsync();
            }
            else
            {
                message = await response.Content.ReadAsStringAsync();
                return StatusCode(500, $"Error calling App2 service: {message}");
            }
            return Ok(message);
        }
    }
}
