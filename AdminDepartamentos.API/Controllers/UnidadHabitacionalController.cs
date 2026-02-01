using AdminDepartamentos.API.Core;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.UnidadHabitacional;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;
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
    [OutputCache(
        PolicyName = "UnidadHabitacionalCache",
        VaryByQueryKeys = new[] { "lastId", "take" }    
    )]
    public async Task<IActionResult> GetAll(int? lastId = null, int take = 20)
    {
        _logger.LogInformation("GETAll UnidadHabitacional - Start.");

        var responseApi = new ResponseAPI<List<UnidadHabitacionalViewModel>>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetUnidadHabitacionales(lastId, take);
            var unidadViewModels =
                unidad.Select(uni => uni.ConvertUnidadHabitacionalViewModelToUnidadHabitacionalModel()).ToList();

            _logger.LogInformation("GETAll UnidadHabitacional - Total encontrados: {Count}.", unidad.Count());
            responseApi.Success = true;
            responseApi.Data = unidadViewModels;
        }
        catch (Exception ex)
        {
            _logger.LogError("GETAll UnidadHabitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET: api/UnidadHabitacionalController/GetById/{id}
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "UnidadHabitacionalCache")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET UnidadHabitacional - Start.");

        var responseApi = new ResponseAPI<UnidadHabitacionalEntity>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetById(id);

            if (unidad is null)
            {
                _logger.LogWarning("GET UnidadHabitacional - Unidad Habitacional with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Unidad Habitacional not found.";
                return NotFound(responseApi);
            }

            _logger.LogInformation("GET UnidadHabitacional - Unidad Habitacional with id {id} found.", id);
            responseApi.Success = true;
            responseApi.Data = unidad;
        }
        catch (Exception ex)
        {
            _logger.LogError("GET UnidadHabitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET: api/UnidadHabitacionalController/Available
    [HttpGet("Available")]
    [OutputCache(
        PolicyName = "UnidadHabitacionalCache",
        VaryByQueryKeys = new[] { "lastId", "take" }    
    )]
    public async Task<IActionResult> GetAvailable(int? lastId = null, int take = 20)
    {
        _logger.LogInformation("GET Available UnidadHabitacional - Start.");

        var responseApi = new ResponseAPI<List<UnidadHabitacionalEntity>>();

        try
        {
            var availableUnits = await _unidadHabitacionalRepository.GetAvailableUnidadHabitacional(lastId, take);

            _logger.LogInformation("GET Available UnidadHabitacional - Total encontrados: {Count}.",
                availableUnits.Count());
            responseApi.Success = true;
            responseApi.Data = availableUnits;
        }
        catch (Exception ex)
        {
            _logger.LogError("GET Available UnidadHabitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET: api/UnidadHabitacionalController/Available
    [HttpGet("Occupied")]
    [OutputCache(
        PolicyName = "UnidadHabitacionalCache",
        VaryByQueryKeys = new[] { "lastId", "take" }    
    )]
    public async Task<IActionResult> GetOccupied(int? lastId = null, int take = 20)
    {
        _logger.LogInformation("GET Occupied UnidadHabitacional - Start.");

        var responseApi = new ResponseAPI<List<UnidadHabitacionalOccuppiedModel>>();

        try
        {
            var unidad = await _unidadHabitacionalRepository.GetOccupiedUnidadHabitacional(lastId, take);
            var occupiedUnits =
                unidad.Select(uni => uni.ConvertUnidadHabitacionalEntityToUnidadHabitacionalOccuppiedModel()).ToList();

            _logger.LogInformation("GET Occupied UnidadHabitacional - Total encontrados: {Count}.",
                occupiedUnits.Count());
            responseApi.Success = true;
            responseApi.Data = occupiedUnits;
        }
        catch (Exception ex)
        {
            _logger.LogError("GET Occupied UnidadHabitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // PUT: api/UnidadHabitacionalController/AssignInquilino
    [HttpPut("AssignInquilino")]
    public async Task<IActionResult> AssignInquilino([FromQuery] int idUnidad, [FromQuery] int idInquilino)
    {
        _logger.LogInformation("Assign Inquilino to Unidad Habitacional - Start.");
        var responseApi = new ResponseAPI<bool>();

        try
        {
            var result = await _unidadHabitacionalRepository.AssignInquilino(idUnidad, idInquilino);

            if (!result)
            {
                _logger.LogWarning(
                    "Error to assign Inquilino to Unidad Habitacional - Unidad Habitacional with id {idUnidad}, Inquilino with id {idInquilino}.",
                    idUnidad, idInquilino);
                responseApi.Success = false;
                responseApi.Message = "No se pudo asignar el inquilino.";
                return NotFound(responseApi);
            }

            await ClearCache();

            _logger.LogInformation("Assign Inquilino to Unidad Habitacional - Success.");
            responseApi.Success = true;
            responseApi.Data = result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Assign Inquilino to Unidad Habitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // PUT: api/UnidadHabitacionalController/Release/{id}
    [HttpPut("Release/{id}")]
    public async Task<IActionResult> Release(int id)
    {
        _logger.LogInformation("Release Unidad Habitacional - Start.");

        var responseApi = new ResponseAPI<bool>();

        try
        {
            var result = await _unidadHabitacionalRepository.ReleaseUnit(id);

            if (!result)
            {
                _logger.LogWarning("Error to release Unidad Habitacional - Unidad Habitacional with id {id}.", id);
                responseApi.Success = false;
                responseApi.Message = "No se pudo liberar la unidad.";
                return NotFound(responseApi);
            }

            await ClearCache();

            _logger.LogInformation("Release Unidad Habitacional - Success.");
            responseApi.Success = true;
            responseApi.Data = result;
        }
        catch (Exception ex)
        {
            _logger.LogError("Release Unidad Habitacional - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // POST: api/UnidadHabitacionalController/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] UnidadHabitacionalDto unidadHabitacionalDto)
    {
        _logger.LogInformation("Save Unidad Habitacional - Start.");

        var responseApi = new ResponseAPI<UnidadHabitacionalDto>();

        var result = await _unidadHabitacionalRepository.Save(unidadHabitacionalDto);
        await ClearCache();

        if (!result.Success)
        {
            _logger.LogWarning("Save Unidad Habitacional - Error. Errors {result.Message}", result.Message);
            responseApi.Success = false;
            responseApi.Message = "Error al guardar el Unidad Habitacional.";
            return BadRequest(responseApi);
        }

        _logger.LogInformation("Save Unidad Habitacional - Unidad Habitacional saved.");
        responseApi.Success = true;
        responseApi.Message = "El Unidad Habitacional se guardo correctamente.";

        return Ok(responseApi);
    }

    // PUT: api/UnidadHabitacionalController/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Updated(int id, [FromBody] UnidadHabitacionalUpdateModel unidadHabitacionalUpdate)
    {
        _logger.LogInformation("Update Unidad Habitacional - Start.");

        var responseApi = new ResponseAPI<UnidadHabitacionalUpdateModel>();

        try
        {
            if (!await _unidadHabitacionalRepository.Exists(cd => cd.IdUnidadHabitacional == id))
            {
                _logger.LogWarning("Update Unidad Habitacional - Unidad Habitacional with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Unidad Habitacional not found.";
                return NotFound(responseApi);
            }

            await _unidadHabitacionalRepository.UpdateUnidadHabitacional(
                id,
                unidadHabitacionalUpdate.Name,
                unidadHabitacionalUpdate.Tipo,
                unidadHabitacionalUpdate.LightCode
            );
            
            await ClearCache();

            _logger.LogInformation("Update Unidad Habitacional - Unidad Habitacional updated.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Update Unidad Habitacional - Error. Errors {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // DELETE: api/UnidadHabitacionalController/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> MarkDeleted(int id)
    {
        _logger.LogInformation("Delete Unidad Habitacional - Start.");

        var responseApi = new ResponseAPI<int>();

        try
        {
            if (!await _unidadHabitacionalRepository.Exists(cd => cd.IdUnidadHabitacional == id))
            {
                _logger.LogWarning("Delete Unidad Habitacional - Unidad Habitacional with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Unidad Habitacional not found.";
                return NotFound(responseApi);
            }

            await _unidadHabitacionalRepository.MarkDeleted(id);
            await ClearCache();

            _logger.LogInformation("Delete Unidad Habitacional - Unidad Habitacional deleted.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Delete Unidad Habitacional - Error. Errors {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
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
    private readonly ILogger<UnidadHabitacionalController> _logger;

    public UnidadHabitacionalController(IUnidadHabitacionalRepository unidadHabitacionalRepository,
        IOutputCacheStore outputCacheStore, ILogger<UnidadHabitacionalController> logger)
    {
        _unidadHabitacionalRepository = unidadHabitacionalRepository;
        _outputCacheStore = outputCacheStore;
        _logger = logger;
    }

    #endregion
}