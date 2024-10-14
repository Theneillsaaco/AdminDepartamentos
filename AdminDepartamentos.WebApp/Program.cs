using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AdminDepartamentos.WebApp;
using AdminDepartamentos.WebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient 
{
    BaseAddress = new Uri("http://localhost:4321"),
    DefaultRequestHeaders = { { "X-Requested-With", "XMLHttpRequest" } }
});

builder.Services.AddScoped<AuthService>();

builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();