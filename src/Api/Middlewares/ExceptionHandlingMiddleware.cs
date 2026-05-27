using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
    {
        try
        {
            await next(ctx);
        }
        catch (ValidationException ex)
        {
            logger.LogInformation(ex, "Validation failed for {Method} {Path}", ctx.Request.Method, ctx.Request.Path);
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            await WriteProblem(ctx, new ValidationProblemDetails(errors)
            {
                Title = "Validation failed",
                Status = StatusCodes.Status422UnprocessableEntity,
                Detail = "One or more validation errors occurred."
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", ctx.Request.Method, ctx.Request.Path);
            await WriteProblem(ctx, new ProblemDetails
            {
                Title = "Unhandled exception",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            });
        }
    }

    private static Task WriteProblem(HttpContext ctx, ProblemDetails problem)
    {
        ctx.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        ctx.Response.ContentType = "application/problem+json";
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(problem, problem.GetType(), JsonOptions));
    }
}
