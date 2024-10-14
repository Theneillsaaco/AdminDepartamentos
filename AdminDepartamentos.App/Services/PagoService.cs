﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdminDepartamentos.App.Models;
using Blazored.LocalStorage;

namespace AdminDepartamentos.App.Services;

public class PagoService
{
    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<ResponseAPI<List<PagoGetModel>>> GetPagos()
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<PagoGetModel>>>("api/Pago/GetPago");
        return response;
    }

    public async Task<ResponseAPI<PagoGetByIdModel>> GetPagoById(int id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.GetFromJsonAsync<ResponseAPI<PagoGetByIdModel>>($"api/Pago/GetById/{id}");
        return response;
    }

    public async Task<ResponseAPI<PagoUpdateModel>> UpdatePago(int id, PagoUpdateModel pagoUpdateModel)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/Pago/Update/{id}", pagoUpdateModel);
        return await response.Content.ReadFromJsonAsync<ResponseAPI<PagoUpdateModel>>();
    }

    public async Task<ResponseAPI<object>> MarkRetrasado(int id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PatchAsync($"api/Pago/Retrasado/{id}", null);
        return await response.Content.ReadFromJsonAsync<ResponseAPI<object>>();
    }
    
    #region Context
    
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    
    public PagoService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }
    
    #endregion
}