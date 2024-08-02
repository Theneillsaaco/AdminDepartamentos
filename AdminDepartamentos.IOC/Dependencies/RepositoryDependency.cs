﻿using AdminDepartament.Infrastructure.Repositories;
using AdminDepartamentos.Domain.Interfaces;
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