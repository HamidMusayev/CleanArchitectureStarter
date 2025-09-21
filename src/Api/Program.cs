using Api.Config;
using Api.Endpoints;
using Api.Extensions;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLayer()
    .AddApiVersioningConfigured()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapHealth();

app.Run();