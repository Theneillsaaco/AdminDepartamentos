using AdminDepartament.Infrastructure.Repositories;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDepartamentos.IOC.Dependencies;

public static class InquilinosDependency
{
    public static void AddInquilinoDependency(this IServiceCollection services)
    {
        // InquilinoRepository
        services.AddScoped<IInquilinoRepository, InquilinoRepository>();
        
        // Services...
    }
}