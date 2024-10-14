using System.Net.Http.Json;
using AdminDepartamentos.WebApp.Models;
using Microsoft.AspNetCore.Components;

namespace AdminDepartamentos.WebApp.Services;

public class AuthService
{
    public async Task Login(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/account/login", loginModel);
        if (response.IsSuccessStatusCode)
            _navigationManager.NavigateTo("/");
        else {}
    }

    public async Task Logout()
    {
        await _httpClient.PostAsync("api/account/logout", null);
        _navigationManager.NavigateTo("/api/Account/login");
    }

    public async Task<bool> IsAuthenticated()
    {
        var response = await _httpClient.GetAsync("api/Account/isauthenticated");
        return response.IsSuccessStatusCode;
    }
    
    #region context
    
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    
    public AuthService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }
    
    #endregion
}