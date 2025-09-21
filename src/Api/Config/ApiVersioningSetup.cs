namespace Api.Config;

public static class ApiVersioningSetup
{
    public static IServiceCollection AddApiVersioningConfigured(this IServiceCollection services)
    {
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
        });
        return services;
    }
}