using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace CalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ITracer tracer;

        public CalculatorController(ITracer tracer)
        {
            this.tracer = tracer;
        }

        [HttpGet("add")]
        public async Task<ActionResult> Addition(int num1, int num2)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = tracer.BuildSpan(actionName).StartActive(true);
            int result = num1 + num2;
            scope.Span.Log($"Requested for addition: {num1} + {num2} = {result}");
            return Ok(result);
        }
    }
}
