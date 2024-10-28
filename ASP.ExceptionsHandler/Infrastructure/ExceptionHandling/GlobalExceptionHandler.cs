using ASP.ExceptionsHandler.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ASP.ExceptionsHandler.Infrastructure.ExceptionHandling
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger,
        IHostEnvironment _environment) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred while processing the request");

            var problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;

            switch (exception)
            {
                case TodoNotFoundException:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    break;

                case InvalidTodoException:
                    problemDetails.Title = exception.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    break;

                default:
                    problemDetails.Title = "An unexpected error occurred";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    break;
            }

            // Add additional details in development environment
            if (_environment.IsDevelopment())
            {
                problemDetails.Detail = exception.ToString();
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}