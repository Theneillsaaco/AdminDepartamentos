using AdminDepartamentos.API.Models;
using AdminDepartamentos.API.Services.BackgroundServices;
using AdminDepartamentos.API.Services.Emails;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.IOC.Dependencies;
using AdminDepartamentos.IOC.Dependencies.Configurations;
using Microsoft.AspNetCore.Antiforgery;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationDependecy(builder.Configuration);

// Add repository and other dependencies.
builder.Services.AddRepositoryDependency();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Host.ConfigureSerilog(builder.Configuration);
builder.Services.ConfigureAntiforgery();

// Register hosted services.
builder.Services.AddHostedService<CheckRetrasosService>();
builder.Services.AddHostedService<EmailServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DepartApi v0.9.1");
        c.ConfigObject.AdditionalItems["withCredentials"] = true;
    });
}
else
{
    app.UseHsts();
    app.UseAntiforgery();
}

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (ctx, http) =>
    {
        ctx.Set("TraceId", http.TraceIdentifier);
        ctx.Set("RequestPath", http.Request.Path);
        ctx.Set("Method", http.Request.Method);
    };
});

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    context.Response.Headers.TryAdd("Permissions-Policy", "geolocation=(), camera=(), microphone=()");
    context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none';");

    await next();   
});

app.UseCors("DefaultCorsPolicy");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();

app.Run();