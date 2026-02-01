using System.Text;
using System.Threading.RateLimiting;
using AdminDepartamentos.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

namespace AdminDepartamentos.IOC.Dependencies.Configurations;

public static class ServicesDepenfency
{
    /// <summary>
    ///     DbContext config.
    /// </summary>
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DepartContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DataBase")));
    }

    /// <summary>
    ///     Identity config.
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 10;
            })
            .AddSignInManager<SignInManager<IdentityUser>>()
            .AddEntityFrameworkStores<DepartContext>()
            .AddDefaultTokenProviders();
    }

    /// <summary>
    ///     Authentication config.
    /// </summary>
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"];
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT Key is not configured.");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("jwt")) context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void ConfigureCORS(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("CORS:Origins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy("DefaultCorsPolicy", builder =>
            {
                builder.WithOrigins(origins ?? [])
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .AllowCredentials()
                    .WithHeaders("Authorization", "Content-Type");
            });
        });

        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                context => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }
                )
            );
        });
    }

    public static void ConfigureAntiforgery(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
        });
    }

    public static void ConfigureSerilog(this IHostBuilder host, IConfiguration configuration)
    {
        host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName();
        });
    }

    /// <summary>
    ///     Cache config.
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(option =>
        {
            option.AddPolicy("InquilinosCache",
                builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("InquilinosCache"));
            option.AddPolicy("PagosCache", builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("PagosCache"));
            option.AddPolicy("UnidadHabitacionalCache",
                builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("UnidadHabitacionalCache"));
            option.AddPolicy("InteresadoCache",
                builder => builder.Expire(TimeSpan.FromMinutes(20)).Tag("InteresadoCache"));
        });
    }

    /// <summary>
    ///     Swagger Config.
    /// </summary>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DepartApi", Version = "v0.9.3" });
        });
    }
}