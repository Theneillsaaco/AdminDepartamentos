using AdminDepartamentos.API.Core;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.InteresadoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AdminDepartamentos.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InteresadoController : ControllerBase
{
    // GET: api/Intersado/GetByType/{type}
    [HttpGet("GetByType/{type}")]
    [OutputCache(PolicyName = "InteresadoCache")]
    public async Task<IActionResult> GetByType(string type)
    {
        _logger.LogInformation("GETByType Interesado - Start.");
        
        var responseApi = new ResponseAPI<List<InteresadoModel>>();

        try
        {
            var interesados = await _interesadoRepository.GetByType(type);
            
            _logger.LogInformation("GETByType Interesado - Total encontrados: {Count}.", interesados.Count());
            responseApi.Success = true;
            responseApi.Data = interesados;
        }
        catch (Exception ex)
        {
            _logger.LogError("GETByType Interesado - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // GET: api/Intersado/GetPending
    [HttpGet("GetPending")]
    [OutputCache(PolicyName = "InteresadoCache")]
    public async Task<IActionResult> GetPending()
    {
        _logger.LogInformation("GETPending Interesado - Start.");
        
        var responseApi = new ResponseAPI<List<Interesado>>();

        try
        {
            var interesados = await _interesadoRepository.GetPendingInteresado();
            
            _logger.LogInformation("GETPending Interesado - Total encontrados: {Count}.", interesados.Count());
            responseApi.Success = true;
            responseApi.Data = interesados;
        }
        catch (Exception ex)
        {
            _logger.LogError("GETPending Interesado - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // GET: api/Interesado/GetById/{id}
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "InteresadoCache")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET Interesado - Start.");
        
        var responseApi = new ResponseAPI<Interesado>();

        try
        {
            var interesado = await _interesadoRepository.GetById(id);

            if (interesado is null)
            {
                _logger.LogWarning("GET Interesado - Interesado with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Interesado not found.";
                return NotFound(responseApi);
            }
            
            _logger.LogInformation("GET Interesado - Interesado with Id {id} found.", id);
            responseApi.Success = true;
            responseApi.Data = interesado;
        }
        catch (Exception ex)
        {
            _logger.LogError("GET Interesado - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi); 
    }
    
    // POST: api/Interesado/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] InteresadoDto interesadoDto)
    {
        _logger.LogInformation("Save Interesado - Start.");
        
        var responseApi = new ResponseAPI<InteresadoDto>();

        var result = await _interesadoRepository.Save(interesadoDto);
        await ClearCache();
        
        if (!result.Success)
        {
            _logger.LogWarning("Save Interesado - Error. Errors {result.Message}", result.Message);
            responseApi.Success = false;
            responseApi.Message = "Error al guardar el Interesado.";
            return BadRequest(responseApi);
        }

        _logger.LogInformation("Save Interesado - Interesado saved.");
        responseApi.Success = true;
        responseApi.Message = "El Interesado se guardo correctamente.";
        
        return Ok(responseApi);
    }
    
    // PUT: api/Interesado/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InteresadoUpdateModel model)
    {
        _logger.LogInformation("Update Interesado - Start.");
        
        var responseApi = new ResponseAPI<bool>();

        try
        {
            if (!await _interesadoRepository.Exists(cd => cd.IdInteresado == id))
            {
                _logger.LogWarning("Update Interesado - Interesado with Id: {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Interesado not found.";
                return NotFound(responseApi);
            }
    
            var interesado = await _interesadoRepository.GetById(id);
            interesado.Update(
                model.FirstName,
                model.LastName,
                model.Telefono,
                model.TipoUnidadHabitacional
            );
            
            await _interesadoRepository.Update(interesado);
            await ClearCache();
            
            _logger.LogInformation("Update Interesado - Interesado updated.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Update Interesado - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // DELETE: api/Interesado/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> MarkDelete(int id)
    {
        _logger.LogInformation("Delete Interesado - Start.");
        
        var responseApi = new ResponseAPI<int>();

        try
        {
            if (!await _interesadoRepository.Exists(cd => cd.IdInteresado == id))
            {
                _logger.LogWarning("Delete Interesado - Interesado with Id: {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Interesado not found.";
                return NotFound(responseApi);
            }
            
            await _interesadoRepository.MarkDeleted(id);
            await ClearCache();
            
            _logger.LogInformation("Delete Interesado - Interesado deleted.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Delete Interesado - Error. Errors {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }

    private async Task ClearCache()
    {
        await _outputCacheStore.EvictByTagAsync("InteresadoCache", default);
    }
        
    #region Fields
    
    private readonly IInteresadoRepository _interesadoRepository;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly ILogger<InteresadoController> _logger;

    public InteresadoController(IInteresadoRepository interesadoRepository, IOutputCacheStore outputCacheStore, ILogger<InteresadoController> logger)
    {
        _interesadoRepository = interesadoRepository;
        _outputCacheStore = outputCacheStore;
        _logger = logger;
    }
    
    #endregion
}