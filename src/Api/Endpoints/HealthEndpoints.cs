using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Api.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealth(this IEndpointRouteBuilder app)
    {
        // Liveness: process is up (no dependencies checked)
        app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

        // Readiness: all dependencies healthy (database, etc.)
        app.MapHealthChecks("/health/ready");

        return app;
    }
}