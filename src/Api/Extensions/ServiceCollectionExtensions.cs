using Api.Filters;
using Api.Middlewares;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services.AddControllers(o => o.Filters.Add<GlobalExceptionFilter>());
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}