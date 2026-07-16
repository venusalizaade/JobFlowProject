using System.Security.Authentication;
using System.Text.Json;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using WebApplication1.Dto.Authentication;
using JobFlowProject.Business.Exceptions.BaseExeption;

namespace WebApplication1.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case BaseBusinessException ex:
                context.Response.StatusCode = ex.Code switch
                {
                    "PermissionDenied_403" => StatusCodes.Status403Forbidden,
                    "ItemNotFound_404" => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new GeneralResponseDto(ex.Message, ex.Code)));
                break;

            case AuthenticationException ex:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new GeneralResponseDto(
                            ex.Message,
                            "AuthenticationError_401")));
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        new GeneralResponseDto(
                            "Something went wrong. Please contact administrator.",
                            "InternalServerError_500")));
                break;
        }
    }
}