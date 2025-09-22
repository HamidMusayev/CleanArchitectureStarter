using System.Reflection;
using Application.Common.Behaviors;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection; // required for AddAutoMapper extensions

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var asm = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(asm));
        services.AddValidatorsFromAssembly(asm);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // AutoMapper registration (no-op config delegate)
        services.AddAutoMapper(cfg => { }, asm);

        return services;
    }
}