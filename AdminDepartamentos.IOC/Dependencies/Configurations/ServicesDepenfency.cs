using System.Text;
using AdminDepartamentos.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AdminDepartamentos.IOC.Dependencies.Configurations;

public static class ServicesDepenfency
{
    /// <summary>
    /// DbContext config.
    /// </summary>
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DepartContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DataBase")));
    }

    /// <summary>
    /// Identity config.
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddEntityFrameworkStores<DepartContext>()
            .AddDefaultTokenProviders();
    }
    
    /// <summary>
    /// Authentication config.
    /// </summary>
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true, 
                    ValidateAudience = false,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
    
    /// <summary>
    /// Cache config.
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(option =>
        {
            option.AddPolicy("InquilinosCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("InquilinosCache"));
            option.AddPolicy("PagosCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("PagosCache"));
            option.AddPolicy("UnidadHabitacionalCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("UnidadHabitacionalCache"));
            option.AddPolicy("InteresadoCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("InteresadoCache"));
        });
    }
    
    /// <summary>
    /// Swagger Config.
    /// </summary>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DepartApi", Version = "v0.8.6" });

            // Configuración para JWT Authentication en Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor ingrese 'Bearer' seguido de un espacio y el token JWT",
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
                        },
                        Scheme = "Bearer",  // Cambiado de 'oauth2' a 'Bearer'
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }); 
    }
}