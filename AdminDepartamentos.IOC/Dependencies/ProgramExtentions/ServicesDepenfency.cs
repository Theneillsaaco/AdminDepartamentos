﻿using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.IOC.Dependencies.ProgramExtentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AdminDepartamentos.IOC.Dependencies.ProgramExtentions;

public static class ServicesDepenfency
{
    /// <summary>
    ///     DbContext config
    /// </summary>
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DepartContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataBase")));
    }

    /// <summary>
    ///     Authentication config
    /// </summary>
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme)
        .AddBearerToken(IdentityConstants.BearerScheme);
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<DepartContext>()
        .AddApiEndpoints();
    }

    public static void ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(option =>
        {
            option.AddPolicy("InquilinosCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("InquilinosCache"));
            option.AddPolicy("PagosCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("PagosCache"));
        });
    }
    
    /// <summary>
    ///     Swagger Confing
    /// </summary>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DepartApi", Version = "v0.8" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor, ingrese el token JWT con el prefijo 'Bearer' en el campo",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            c.SchemaFilter<SchemaFilter>();
        });
    }
}