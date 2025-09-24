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
        var responseApi = new ResponseAPI<List<InteresadoModel>>();

        try
        {
            var interesados = await _interesadoRepository.GetByType(type);
            
            responseApi.Success = true;
            responseApi.Data = interesados;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // GET: api/Intersado/GetPending
    [HttpGet("GetPending")]
    [OutputCache(PolicyName = "InteresadoCache")]
    public async Task<IActionResult> GetPending()
    {
        var responseApi = new ResponseAPI<List<Interesado>>();

        try
        {
            var interesados = await _interesadoRepository.GetPendingInteresado();
            
            responseApi.Success = true;
            responseApi.Data = interesados;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // GET: api/Interesado/GetById/{id}
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "InteresadoCache")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseApi = new ResponseAPI<Interesado>();

        try
        {
            var interesado = await _interesadoRepository.GetById(id);

            if (interesado is null)
            {
                responseApi.Success = false;
                responseApi.Message = "Interesado not found.";
                return NotFound(responseApi);
            }
            
            responseApi.Success = true;
            responseApi.Data = interesado;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi); 
    }
    
    // POST: api/Interesado/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] InteresadoDto interesadoDto)
    {
        var responseApi = new ResponseAPI<InteresadoDto>();

        var result = await _interesadoRepository.Save(interesadoDto);
        await ClearCache();
        responseApi.Success = result.Success;
        responseApi.Message = result.Message;
        
        return Ok(responseApi);
    }
    
    // PUT: api/Interesado/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InteresadoUpdateModel interesadoUpdateModel)
    {
        var responseApi = new ResponseAPI<bool>();

        try
        {
            if (!await _interesadoRepository.Exists(cd => cd.IdInteresado == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Interesado with Id: {id} not found.";
                return NotFound(responseApi);
            }
    
            var interesado = interesadoUpdateModel.ConvertInteresadoUpdateModelToInteresadoEntity();
            interesado.IdInteresado = id;
            
            await _interesadoRepository.Update(interesado);
            await ClearCache();
            
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // DELETE: api/Interesado/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> MarkDelete(int id)
    {
        var responseApi = new ResponseAPI<int>();

        try
        {
            if (!await _interesadoRepository.Exists(cd => cd.IdInteresado == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Interesado with Id: {id} not found.";
                return NotFound(responseApi);
            }
            
            await _interesadoRepository.MarkDeleted(id);
            await ClearCache();
            
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
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
    
    public InteresadoController(IInteresadoRepository interesadoRepository, IOutputCacheStore outputCacheStore)
    {
        _interesadoRepository = interesadoRepository;
        _outputCacheStore = outputCacheStore;
    }
    
    #endregion
}