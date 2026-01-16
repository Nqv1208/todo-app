using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Identity.Options;
using TodoApp.Infrastructure.Identity.Services;

namespace Microsoft.Extensions.DependencyInjection;

// Infrastructure Layer Dependency Injection
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        // Database - PostgreSQL
        var connectionString = builder.Configuration.GetConnectionString("TodoAppDb");
        
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

        // Register DbContext as IApplicationDbContext
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        // Identity Services
        services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

        var jwtSettings = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtSettings>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings!.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                    ),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Database Initialiser
        services.AddScoped<ApplicationDbContextInitialiser>();

    }

    // // Apply pending migrations at startup (Development only)
    // public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    // {
    //     using var scope = serviceProvider.CreateScope();
    //     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
    //     await context.Database.MigrateAsync();
    // }
}
