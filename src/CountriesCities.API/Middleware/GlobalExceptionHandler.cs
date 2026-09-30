using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CountriesCities.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        var errorResponse = new CountriesCities.Application.Common.ErrorResponse
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Message = "An unexpected error occurred while processing your request."
        };

        httpContext.Response.StatusCode = errorResponse.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        
        return true;
    }
}
