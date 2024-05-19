using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.OpenApi.Models;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;

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
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserRequest", Version = "v1" }));
            services.AddHttpClient("CalculatorService",
                    c =>
                    {
                        c.BaseAddress =
                            // Value set by tye
                            Configuration.GetServiceUri("CalculatorService")
                            // For running project without tye
                            ?? new Uri(Configuration["CalculatorService"]);
                    })
                .ConfigurePrimaryHttpMessageHandler(() =>
                    new HttpClientHandler
                    {
                        // For demo only. Use appropriate certificates in production.
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    });

            services.AddOpenTracing();
            // Adds the Jaeger Tracer.
            string jaegerService = Configuration.GetValue<string>("JaegerServiceEndpoint");
            services.AddSingleton<ITracer>(sp =>
            {
                var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(
                        new HttpSender(
                            // Value set by Tye
                            Configuration.GetConnectionString("Jaeger", "http-thrift")
                            // For running project without Tye
                            ?? jaegerService))
                    .Build();

                var tracer = new Tracer.Builder(serviceName)
                    // The constant sampler reports every span.
                    .WithSampler(new ConstSampler(true))
                    // LoggingReporter prints every reported span to the logging framework.
                    .WithReporter(reporter)
                    .Build();
                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options =>
                options.OperationNameResolver =
                    request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
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