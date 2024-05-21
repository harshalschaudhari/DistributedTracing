using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OpenTracing.Propagation;

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

            Dictionary<string, IEnumerable<string>> requestHeader = Request.Headers.ToDictionary(a => a.Key, a => (IEnumerable<string>)a.Value);
            
            var requestHeaderDictionary = new Dictionary<string, string>();
             
            if (scope.Span != null)
            {
                foreach (var header in requestHeader)
                {
                    scope.Span.SetTag(header.Key, string.Join(",", header.Value.ToArray()));
                    requestHeaderDictionary.Add(header.Key, string.Join(",", header.Value.ToArray()));

                    Console.WriteLine("Request Header - " + header.Key, string.Join(",", header.Value.ToArray()));
                }
            }
            
            tracer.Inject(scope.Span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(requestHeaderDictionary));

            var client = httpClientFactory.CreateClient("CalculatorService");
            foreach (var entry in requestHeaderDictionary)
                client.DefaultRequestHeaders.Add(entry.Key, entry.Value);

            var response = await client.GetAsync(requestUrl);
            return response.IsSuccessStatusCode
                ? Ok(Convert.ToDouble(await response.Content.ReadAsStringAsync()))
                : Problem("Log service failed");
        }
    }
}
