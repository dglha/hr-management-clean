using System.Net;
using HR.LeaveManagement.API.Models;
using HR.LeaveManagement.Application.Exceptions;

namespace HR.LeaveManagement.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(httpContext, e);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomValidationProblemDetails problem = new();

        switch (exception)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomValidationProblemDetails
                {
                    Title = badRequestException.Message,
                    Status = (int)statusCode,
                    Detail = badRequestException.InnerException?.Message,
                    Type = nameof(BadRequestException),
                    Errors = badRequestException.ValidationErrors
                };
                break;
            case NotFoundException NotFound:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomValidationProblemDetails
                {
                    Title = NotFound.Message,
                    Status = (int)statusCode,
                    Detail = NotFound.InnerException?.Message,
                    Type = nameof(BadRequestException),
                };
                break;
            default:
                problem = new CustomValidationProblemDetails()
                {
                    Title = exception.Message,
                    Status = (int)statusCode,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = exception.StackTrace,
                };
                break;
        }
        
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}