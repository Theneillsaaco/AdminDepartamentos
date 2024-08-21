using AdminDepartamentos.API.Middleware;
using AdminDepartamentos.API.Models;
using AdminDepartamentos.API.Services.BackgroundServices;
using AdminDepartamentos.API.Services.Emails;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.IOC.Dependencies;
using AdminDepartamentos.IOC.Dependencies.ProgramExtentions;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddRepositoryDependency();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHostedService<CheckRetrasosService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddHostedService<EmailServices>();

builder.Services.ConfigureAuthentication();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureOutputCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RegisterAuthorizationMiddleware>();
app.MapIdentityApi<IdentityUser>();

app.UseOutputCache();

app.UseCors("NewPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();