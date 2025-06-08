using AdminDepartamentos.API.Core;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.UnidadHabitacional;
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
public class UnidadHabitacionalController : ControllerBase
{
    // GET: api/UnidadHabitacionalController/GetAll
    [HttpGet("GetAll")]
    [OutputCache(PolicyName = "UnidadHabitacionalCache")]
    public async Task<IActionResult> GetAll()
    {
        var responseApi = new ResponseAPI<List<UnidadHabitacionalViewModel>>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetUnidadHabitacionales();
            var unidadViewModels =
                unidad.Select(uni => uni.ConvertUnidadHabitacionalViewModelToUnidadHabitacionalModel()).ToList();
            
            responseApi.Success = true;
            responseApi.Data = unidadViewModels;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi); 
    }

    // GET: api/UnidadHabitacionalController/GetById/{id}
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "UnidadHabitacionalCache")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseApi = new ResponseAPI<UnidadHabitacional>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetById(id);

            if (unidad is null)
            {
                responseApi.Success = false;
                responseApi.Message = $"Unidad Habitacional with Id {id} not found.";
                return NotFound(responseApi);
            }

            responseApi.Success = true;
            responseApi.Data = unidad;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return  StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // GET: api/UnidadHabitacionalController/Available
    [HttpGet("Available")]
    [OutputCache(PolicyName = "UnidadHabitacionalCache")]
    public async Task<IActionResult> GetAvailable()
    {
        var responseApi = new ResponseAPI<List<UnidadHabitacional>>();

        try
        {
            var availableUnits = await _unidadHabitacionalRepository.GetAvailableUnidadHabitacional();
            
            responseApi.Success = true;
            responseApi.Data = availableUnits;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }
    
    // GET: api/UnidadHabitacionalController/Available
    [HttpGet("Occupied")]
    [OutputCache(PolicyName = "UnidadHabitacionalCache")]
    public async Task<IActionResult> GetOccupied()
    {
        var responseApi = new ResponseAPI<List<UnidadHabitacionalOccuppiedModel>>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetOccupiedUnidadHabitacional();
            var occupiedUnits =
                unidad.Select(uni => uni.ConvertUnidadHabitacionalEntityToUnidadHabitacionalOccuppiedModel()).ToList();
                
            responseApi.Success = true;
            responseApi.Data = occupiedUnits;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }
        
        return Ok(responseApi);
    }
    
    // PUT: api/UnidadHabitacionalController/AssignInquilino
    [HttpPut("AssignInquilino")]
    public async Task<IActionResult> AssignInquilino([FromQuery] int idUnidad, [FromQuery] int idInquilino)
    {
        var responseApi = new ResponseAPI<bool>();

        try
        {
            var result = await _unidadHabitacionalRepository.AssignInquilino(idUnidad, idInquilino);
            
            if (!result)
            {
                responseApi.Success = false;
                responseApi.Message = "No se pudo asignar el inquilino.";
                return NotFound(responseApi);
            }

            await ClearCache();
            responseApi.Success = true;
            responseApi.Data = result;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // PUT: api/UnidadHabitacionalController/Release/{id}
    [HttpPut("Release/{id}")]
    public async Task<IActionResult> Release(int id)
    {
        var responseApi = new ResponseAPI<bool>();

        try
        {
            var result = await _unidadHabitacionalRepository.ReleaseUnit(id);

            if (!result)
            {
                responseApi.Success = false;
                responseApi.Message = "No se pudo liberar la unidad.";
                return NotFound(responseApi);
            }

            await ClearCache();
            responseApi.Success = true;
            responseApi.Data = result;
        }
        catch(Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }
    
    // POST: api/UnidadHabitacionalController/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] UnidadHabitacionalDto unidadHabitacionalDto)
    {
        var responseApi = new ResponseAPI<UnidadHabitacionalDto>();

        var result = await _unidadHabitacionalRepository.Save(unidadHabitacionalDto);
        await ClearCache();
        
        responseApi.Success = result.Success;
        responseApi.Message = result.Message;

        return Ok(responseApi);
    }

    // PUT: api/UnidadHabitacionalController/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Updated(int id, [FromBody] UnidadHabitacionalUpdateModel unidadHabitacionalUpdate)
    {
        var responseApi = new ResponseAPI<UnidadHabitacionalUpdateModel>();

        try
        {
            if (!await _unidadHabitacionalRepository.Exists(cd => cd.IdUnidadHabitacional == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Unidad Habitacional with Id: {id} not found.";

                return NotFound(responseApi);
            }

            var unidad = unidadHabitacionalUpdate.ConvertUnidadHabitacionalEntityToUnidadHabitacionalUpdateModel();
            unidad.IdUnidadHabitacional = id;

            await _unidadHabitacionalRepository.Update(unidad);
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

    // DELETE: api/UnidadHabitacionalController/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> MarkDeleted(int id)
    {
        var responseApi = new ResponseAPI<int>();

        try
        {
            if (!await _unidadHabitacionalRepository.Exists(cd => cd.IdUnidadHabitacional == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Unidad Habitacional with Id: {id} not found.";
                return NotFound(responseApi);
            }

            await _unidadHabitacionalRepository.MarkDeleted(id);
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
        await _outputCacheStore.EvictByTagAsync("UnidadHabitacionalCache", default);
    }

    #region Fields
    
    private readonly IUnidadHabitacionalRepository _unidadHabitacionalRepository;
    private readonly IOutputCacheStore _outputCacheStore;
    
    public UnidadHabitacionalController(IUnidadHabitacionalRepository unidadHabitacionalRepository, IOutputCacheStore outputCacheStore)
    {
        _unidadHabitacionalRepository = unidadHabitacionalRepository;
        _outputCacheStore = outputCacheStore;
    }

    #endregion
}
