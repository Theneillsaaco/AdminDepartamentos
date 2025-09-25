using AdminDepartamentos.API.Models;
using AdminDepartamentos.API.Services.BackgroundServices;
using AdminDepartamentos.API.Services.Emails;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.IOC.Dependencies;
using AdminDepartamentos.IOC.Dependencies.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfigurationDependecy(builder.Configuration);

// Add repository and other dependencies.
builder.Services.AddRepositoryDependency();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

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
    app.UseSwaggerUI();
    
    // Tests.
    var serviceProvider = builder.Services.BuildServiceProvider();
    var logger = serviceProvider.GetService<ILogger<Program>>();
    logger.LogInformation("Services registered successfully");
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("DefaultCorsPolicy");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();

app.Run();