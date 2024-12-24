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
    [HttpGet]
    [Route("GetPago")]
    [OutputCache(PolicyName = "PagosCache")]
    public async Task<IActionResult> GetPago()
    {
        var responseApi = new ResponseAPI<List<PagoGetByInquilinoModel>>();

        try
        {
            var pagos = await _pagoRepository.GetPago();

            responseApi.Success = true;
            responseApi.Data = pagos.Select(pa => pa.ConvertToPagoGetByInquilinoModel()).ToList();
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET api/<PagoController>/5
    [HttpGet]
    [Route("GetById/{id}")]
    [OutputCache(PolicyName = "PagosCache")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseApi = new ResponseAPI<PagoGetModel>();

        try
        {
            var pago = await _pagoRepository.GetById(id);
            
            responseApi.Success = true;
            responseApi.Data = pago.ConvertToPagoGetModel();
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
        }

        return Ok(responseApi);
    }

    // PUT api/<PagoController>/5
    [HttpPut]
    [Route("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PagoUpdateModel pagoUpdateModel)
    {
        var responseApi = new ResponseAPI<PagoUpdateModel>();

        try
        {
            if (!await _pagoRepository.Exists(pa => pa.IdPago == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Pago with Id {id} not found.";
                return NotFound(responseApi);
            }

            var pago = await _pagoRepository.GetById(id);
            _pagoRepository.DetachEntity(pago);

            var updatedPago = pagoUpdateModel.ConverToPagoEntityToPagoUpdateModel();
            updatedPago.IdPago = id;
            updatedPago.IdInquilino = pago.IdInquilino;

            await _pagoRepository.Update(updatedPago);
            await ClearCacheAsync();
            
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

    [HttpPatch]
    [Route("Retrasado/{id}")]
    public async Task<IActionResult> MarkRetrasado(int id)
    {
        var responseApi = new ResponseAPI<object>();

        try
        {
            var pago = await _pagoRepository.GetById(id);
            if (pago is null)
            {
                responseApi.Success = false;
                responseApi.Message = "Pago not found.";
                return NotFound(responseApi);
            }

            pago.Retrasado = false;
            pago.Email = false;

            await _pagoRepository.Update(pago);
            await ClearCacheAsync();
            
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
    
    private async Task ClearCacheAsync()
    {
        await _outputCacheStore.EvictByTagAsync("PagosCache", default);
    }

    #region Fields

    private readonly IPagoRepository _pagoRepository;
    private readonly IOutputCacheStore _outputCacheStore;

    public PagoController(IPagoRepository pagoRepository, IOutputCacheStore outputCacheStore)
    {
        _pagoRepository = pagoRepository;
        _outputCacheStore = outputCacheStore;
    }

    #endregion
}