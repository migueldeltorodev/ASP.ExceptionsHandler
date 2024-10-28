using ASP.ExceptionsHandler;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        try
        {
            Log.Information("starting server.");

            var builder = WebApplication.CreateBuilder(args);

            //we take the configuration from StartupExtensions.cs
            var app = builder
                .ConfigureServices()
                .ConfigurePipeline();

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