using Jaeger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using System.Net.Http;

namespace UserRequestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {

        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITracer tracer;

        public UserRequestController(IHttpClientFactory httpClientFactory, ITracer tracer)
        {
            this.httpClientFactory = httpClientFactory;
            this.tracer = tracer;
        }

        [HttpGet("add")]
        public async Task<ActionResult> Addition(int num1, int num2)
        {
            string requestUrl = $"api/Calculator/add?num1={num1}&num2={num2}";
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = tracer.BuildSpan(actionName).StartActive(true);
            var client = httpClientFactory.CreateClient("CalculatorService");
            var response = await client.GetAsync(requestUrl);
            return response.IsSuccessStatusCode
                ? Ok(Convert.ToDouble(await response.Content.ReadAsStringAsync()))
                : Problem("Log service failed");
        }
    }
}
