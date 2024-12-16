using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDepartamentos.IOC.Dependencies;

public static class RepositoryDependency
{
    public static void AddRepositoryDependency(this IServiceCollection services)
    {
        services.AddScoped<IInquilinoRepository, InquilinoRepository>()
            .AddScoped<IPagoRepository, PagoRepository>();

        // Services...
    }
}