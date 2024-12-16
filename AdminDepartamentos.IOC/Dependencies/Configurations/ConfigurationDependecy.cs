using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDepartamentos.IOC.Dependencies.Configurations;

public static class ConfigurationDependecy
{
    public static void AddConfigurationDependecy(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();
        services.ConfigureAuthentication(configuration);
        services.ConfigureSwagger();
        services.ConfigureOutputCache();
    }
}