using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

// Application Layer Dependency Injection
public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        // MediatR
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // AutoMapper
        services.AddAutoMapper(cfg => 
            cfg.AddMaps(Assembly.GetExecutingAssembly()));
    }
}
