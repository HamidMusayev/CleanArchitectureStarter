using System.Text.Json;

namespace Api.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            ctx.Response.ContentType = "application/problem+json";
            var payload = new { title = "Unhandled exception", detail = ex.Message, status = 500 };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}