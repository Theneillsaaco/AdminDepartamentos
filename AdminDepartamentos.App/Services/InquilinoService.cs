using System.Net.Http.Headers;
using System.Net.Http.Json;
using AdminDepartamentos.App.Models;
using AdminDepartamentos.App.Models.InquilinoModels;
using Blazored.LocalStorage;

namespace AdminDepartamentos.App.Services;

public class InquilinoService
{
    public async Task<ResponseAPI<List<InquilinoGetModel>>> GetInquilino()
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ResponseAPI<List<InquilinoGetModel>>>("api/inquilino/GetAll");
    }

    public async Task<ResponseAPI<InquilinoGetByIdModel>> GetInquilinoById(int id)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetFromJsonAsync<ResponseAPI<InquilinoGetByIdModel>>($"api/inquilino/GetById/{id}");
    }

    public async Task<ResponseAPI<InquilinoSaveModel>> saveInquilino(InquilinoSaveModel inquilino)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/inquilino/save", inquilino);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<ResponseAPI<InquilinoSaveModel>>();
        else
        {
            return new ResponseAPI<InquilinoSaveModel>
            {
                Success = false,
                Message = $"Error al guardar el inquilino: {response.ReasonPhrase}",
            };
        }
    }
    
    public async Task<ResponseAPI<InquilinoUpdateModel>> UpdateInquilino(int id, InquilinoUpdateModel model)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/inquilino/Update/{id}", model);
        return await response.Content.ReadFromJsonAsync<ResponseAPI<InquilinoUpdateModel>>();
    }

    public async Task<ResponseAPI<object>> DeleteInquilino(int id)
    {
        await SetAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/inquilino/Delete/{id}");
        return await response.Content.ReadFromJsonAsync<ResponseAPI<object>>();
    }
    
    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    #region Fields
    
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    
    public InquilinoService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }
    
    #endregion
}