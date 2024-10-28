using ASP.ExceptionsHandler.Infrastructure.ExceptionHandling;
using ASP.ExceptionsHandler.Services;
using DotNetEnv;
using Serilog;

namespace ASP.ExceptionsHandler
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            Env.Load();

            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.WriteTo.Console();
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            });

            // Configurate serilog from appsettings.json
            var logPath = Environment.GetEnvironmentVariable("LOG_PATH");
            var serverUrl = Environment.GetEnvironmentVariable("SEQ_SERVER_URL");

            // Modificar la configuración de Serilog
            builder.Configuration["Serilog:WriteTo:0:Args:path"] = logPath;
            builder.Configuration["Serilog:WriteTo:1:Args:serverUrl"] = serverUrl;

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register services
            builder.Services.AddScoped<ITodoService, TodoService>();

            // Register exception handler
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            //Optional: Enable CORS
            builder.Services.AddCors(options => options.AddPolicy(
                "open",
                policy => policy.WithOrigins(
                    builder.Configuration["ApiUrl"] ?? "https://localhost:7020")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true)
            ));

            // Optional: Enable Swagger for API documentation
            builder.Services.AddSwaggerGen();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseCors("open");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseAuthorization();
            app.UseExceptionHandler();
            app.MapControllers();

            return app;
        }
    }
}