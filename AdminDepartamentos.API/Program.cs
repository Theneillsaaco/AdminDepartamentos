using AdminDepartamentos.API.Models;
using AdminDepartamentos.API.Services.BackgroundServices;
using AdminDepartamentos.API.Services.Emails;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.IOC.Dependencies;
using AdminDepartamentos.IOC.Dependencies.ProgramExtentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureOutputCache();

// Add repository and other dependencies.
builder.Services.AddRepositoryDependency();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

// Register hosted services.
builder.Services.AddHostedService<CheckRetrasosService>();
builder.Services.AddHostedService<EmailServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Tests.
var serviceProvider = builder.Services.BuildServiceProvider();
var logger = serviceProvider.GetService<ILogger<Program>>();
logger.LogInformation("Services registered successfully");

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .WithOrigins("http://localhost:54321")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();

app.Run();