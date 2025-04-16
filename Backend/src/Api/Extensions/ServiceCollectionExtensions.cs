using Application;
using Application.Behaviors;
using Application.Data.Abstractions;
using Application.Services;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Support.Cqrs;
using Support.Cqrs.Abstractions;
using System.Reflection;
using System.Text;
using Infrastructure.Contexts;
using Infrastructure.Interceptors;
using Infrastructure.Repositories;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register HttpContextAccessor and interceptor
        services.AddHttpContextAccessor();
        services.AddScoped<AuditableEntityInterceptor>();

        // Register the DbContext with Postgres provider and interceptor
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditableEntityInterceptor>());
        });
        
        // Register repositories and repository factory for access in singleton services
        services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        // Register authentication services
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
                    ValidateIssuerSigningKey = true,
                };
            });

        // Register application layer services
        services.AddServices(typeof(ApplicationProjectReference).Assembly);

        // Register OpenAPI if needed
        services.AddOpenApi();

        // Add controllers
        services.AddControllers();

        // Add custom mediator, pipeline behaviours and handlers from the Application layer
        services.AddSingleton<IMediator, Mediator>();
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        services.AddHandlers(typeof(ApplicationProjectReference).Assembly);

        return services;
    }
    
    private static void AddServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IServiceFactory, ServiceFactory>();
        
        foreach (var assembly in assemblies)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.GetInterfaces()
                    .Any(i => i.GetInterfaces().Contains(typeof(IServiceBase))))
                .ToList();

            foreach (var type in serviceTypes)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.GetInterfaces().Contains(typeof(IServiceBase)));

                foreach (var itf in interfaces)
                    services.AddTransient(itf, type);
            }
        }
    }

    private static void AddHandlers(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                     i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                .ToList();

            foreach (var type in handlerTypes)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                                 i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

                foreach (var itf in interfaces)
                    services.AddTransient(itf, type);
            }
        }
    }
}