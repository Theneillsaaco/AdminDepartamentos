using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdminDepartamentos.App.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace AdminDepartamentos.App.Services;

public class AuthService
{
    public async Task<bool> IsAuthenticated()
    {
        var token = await _localStorageService.GetItemAsync<string>("authToken");

        if (string.IsNullOrEmpty(token))
            return false;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("api/Account/isauthenticated");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result?.IsAuthenticated ?? false;
        }

        return false;
    }
    
    #region Fields
    
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorageService;

    public AuthService(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
    }
    
    #endregion
}