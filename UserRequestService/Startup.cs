using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UserRequestService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string calculatorService = Configuration.GetValue<string>("CalculatorService");
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserRequest", Version = "v1" }));
            services.AddHttpClient("CalculatorService",
                c =>
                {
                    c.BaseAddress = new Uri(calculatorService);
                })
              .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    // For demo only. Use appropriate certificates in production.
                    ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });

            // Read Jaeger endpoint from appsettings.json 
            string jaegerEndpoint = Configuration.GetValue<string>("JaegerServiceEndpoint");
            string serviceName = Configuration.GetValue<string>("jaegerServiceName");

            services.AddOpenTelemetryTracing(
             (builder) => builder
                 .AddAspNetCoreInstrumentation()
                 .AddHttpClientInstrumentation()
                 .AddJaegerExporter(j => { j.AgentHost = serviceName; j.AgentPort = 6831; })
                 .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("UserRequest"))
           );  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserRequestService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}