using ASP.ExceptionsHandler.Infrastructure.ExceptionHandling;
using ASP.ExceptionsHandler.Services;
using DotNetEnv;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        Env.Load();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        try
        {
            Log.Information("starting server.");
            var builder = WebApplication.CreateBuilder(args);
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseAuthorization();
            app.UseExceptionHandler();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "server terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}