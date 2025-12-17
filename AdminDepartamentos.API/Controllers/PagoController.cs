using AdminDepartamentos.API.Core;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.PagoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.OutputCaching;

namespace AdminDepartamentos.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PagoController : ControllerBase
{
    // GET: api/<PagoController>
    [HttpGet("GetPago")]
    [OutputCache(PolicyName = "PagosCache")]
    public async Task<IActionResult> GetPago()
    {
        _logger.LogInformation("GETAll Pago - Start."); 
        
        var responseApi = new ResponseAPI<List<PagoGetByInquilinoModel>>();

        try
        {
            var pagos = await _pagoRepository.GetPago();

            _logger.LogInformation("GETAll Pago - Total encontrados: {Count}.", pagos.Count());
            responseApi.Success = true;
            responseApi.Data = pagos.Select(pa => pa.ConvertToPagoGetByInquilinoModel()).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("GETAll Pago - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET api/<PagoController>/5
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "PagosCache")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET Pago - Start.");
        
        var responseApi = new ResponseAPI<PagoGetModel>();

        try
        {
            var pago = await _pagoRepository.GetById(id);
            
            if (pago is null)
            {
                _logger.LogWarning("GET Pago - Pago with id {id} not found.", id);  
                responseApi.Success = false;
                responseApi.Message = "Pago not found.";
                return NotFound(responseApi);
            }
            
            _logger.LogInformation("GET Pago - Pago with Id {id} found.", id);
            responseApi.Success = true;
            responseApi.Data = pago.ConvertToPagoGetModel();
        }
        catch (Exception ex)
        {
            _logger.LogError("GET Pago - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
        }

        return Ok(responseApi);
    }

    // PUT api/<PagoController>/5
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PagoUpdateModel pagoUpdateModel)
    {
        _logger.LogInformation("Update Pago - Start.");
        var responseApi = new ResponseAPI<PagoUpdateModel>();

        try
        {
            if (!await _pagoRepository.Exists(pa => pa.IdPago == id))
            {
                _logger.LogWarning("Update Pago - Pago with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Pago not found.";
                return NotFound(responseApi);
            }

            var pago = await _pagoRepository.GetById(id);
            _pagoRepository.DetachEntity(pago);

            var updatedPago = pagoUpdateModel.ConverToPagoEntityToPagoUpdateModel();
            updatedPago.IdPago = id;
            updatedPago.IdInquilino = pago.IdInquilino;

            await _pagoRepository.Update(updatedPago);
            await ClearCacheAsync();
            
            _logger.LogInformation("Update Pago - Pago updated.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Update Pago - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    [HttpPatch("Retrasado/{id}")]
    public async Task<IActionResult> MarkRetrasado(int id)
    {
        _logger.LogInformation("Mark Pago as Retrasado - Start.");
        var responseApi = new ResponseAPI<object>();

        try
        {
            var pago = await _pagoRepository.GetById(id);
            if (pago is null)
            {
                _logger.LogWarning("Mark Pago as Retrasado - Pago with Id {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Pago not found.";
                return NotFound(responseApi);
            }

            pago.Retrasado = false;
            pago.Email = false;

            await _pagoRepository.Update(pago);
            await ClearCacheAsync();
            
            _logger.LogInformation("Mark Pago as Retrasado - Pago updated.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Mark Pago as Retrasado - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }
    
    private async Task ClearCacheAsync()
    {
        await _outputCacheStore.EvictByTagAsync("PagosCache", default);
    }

    #region Fields

    private readonly IPagoRepository _pagoRepository;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly ILogger<PagoController> _logger;

    public PagoController(IPagoRepository pagoRepository, IOutputCacheStore outputCacheStore, ILogger<PagoController> logger)
    {
        _pagoRepository = pagoRepository;
        _outputCacheStore = outputCacheStore;
        _logger = logger;
    }

    #endregion
}