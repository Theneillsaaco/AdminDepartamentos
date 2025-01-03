using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AdminDepartamentos.App;
using AdminDepartamentos.App.Services;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient{ BaseAddress = new Uri("http://localhost:4321") });

// Dependencies
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSweetAlert2();

// Authorizacion Dependencies
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

// Services
builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<InquilinoService>();

await builder.Build().RunAsync();