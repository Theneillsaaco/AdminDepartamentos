using AdminDepartament.Infrastructure.Repositories;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDepartamentos.IOC.Dependencies;

public static class PagoDependency
{
    public static void AddPagoDependency(this IServiceCollection service)
    {
        // PagoRepository
        service.AddScoped<IPagoRepository, PagoRepository>();
        
        // Services...
    }
}