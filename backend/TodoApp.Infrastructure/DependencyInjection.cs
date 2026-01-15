using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Domain.Common.Interfaces;
using TodoApp.Domain.Identity.Interfaces;
using TodoApp.Domain.Workspace.Interfaces;
using TodoApp.Domain.Content.Interfaces;
using TodoApp.Domain.Activity.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Data.Repositories;
using TodoApp.Infrastructure.Identity.Services;

namespace TodoApp.Infrastructure;

// Infrastructure Layer Dependency Injection
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database - PostgreSQL
        var connectionString = configuration.GetConnectionString("TodoAppDb");
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IContentItemRepository, ContentItemRepository>();
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

        // Identity Services
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    // Apply pending migrations at startup (Development only)
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.MigrateAsync();
    }
}
