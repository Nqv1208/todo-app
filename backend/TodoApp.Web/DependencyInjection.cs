using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        // Add web-specific services here
        
        return services;
    }
}
