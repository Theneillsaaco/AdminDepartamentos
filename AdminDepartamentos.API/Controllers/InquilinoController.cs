using AdminDepartamentos.API.Core;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AdminDepartamentos.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InquilinoController : ControllerBase
{
    // GET: api/InquilinoController/GetAll
    [HttpGet("GetAll")]
    [OutputCache(PolicyName = "InquilinosCache")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GETAll Inquilinos - Start.");

        var responseApi = new ResponseAPI<List<InquilinoViewModel>>();

        try
        {
            var inquilinos = await _inquilinoRepository.GetInquilinos();

            var inquilinoViewModels =
                inquilinos.Select(inq => inq.ConvertInquilinoEntityToInquilinoViewModel()).ToList();

            _logger.LogInformation("GETAll Inquilinos - Total encontrados: {Count}.", inquilinos.Count());
            responseApi.Success = true;
            responseApi.Data = inquilinoViewModels;
        }
        catch (Exception ex)
        {
            _logger.LogError("GETAll Inquilinos - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET: api/InquilinoController/GetById/{id}
    [HttpGet("GetById/{id}")]
    [OutputCache(PolicyName = "InquilinosCache")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET Inquilino - Start.");

        var responseApi = new ResponseAPI<InquilinoEntity>();

        try
        {
            var inquilino = await _inquilinoRepository.GetById(id);
            
            _logger.LogInformation("GET Inquilino - Inquilino with id {id} found.", id);
            responseApi.Success = true;
            responseApi.Data = inquilino;
        }
        catch (Exception ex)
        {
            _logger.LogError("GET Inquilino - Error. Errors: {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // POST: api/InquilinoController/Save
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] InquilinoSaveModel inquilinoSaveModel)
    {
        _logger.LogInformation("Save Inquilino - Start.");

        var responseApi = new ResponseAPI<InquilinoSaveModel>();

        if (inquilinoSaveModel.InquilinoDto is null || inquilinoSaveModel.PagoDto is null)
        {
            _logger.LogWarning(
                "Save Inquilino - Invalid data. Inquilino: {inquilinoSaveModel.inquilinoDto}, Pago: {inquilinoSaveModel.PagoDto}",
                inquilinoSaveModel.InquilinoDto, inquilinoSaveModel.PagoDto);
            responseApi.Success = false;
            responseApi.Message = "Datos invalidos.";
            return BadRequest(responseApi);
        }
        
        var result = await _inquilinoRepository.Save(inquilinoSaveModel.InquilinoDto, inquilinoSaveModel.PagoDto);

        if (!result.Success)
        {
            _logger.LogWarning("Save Inquilino - Error. Errors {result.Message}", result.Message);
            responseApi.Success = false;
            responseApi.Message = "Error al guardar el Inquilino.";
            return BadRequest(responseApi);
        }

        await ClearCacheAsync();
        _logger.LogInformation("Save Inquilino - Inquilino saved.");
        responseApi.Success = true;
        responseApi.Message = "El Inquilino se guardo correctamente.";

        return Ok(responseApi);
    }

    // PUT: api/InquilinoController/Update/{id}
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InquilinoUpdateModel model)
    {
        _logger.LogInformation("Update Inquilino - Start.");

        var responseApi = new ResponseAPI<InquilinoUpdateModel>();

        try
        {
            if (!await _inquilinoRepository.Exists(cd => cd.IdInquilino == id))
            {
                _logger.LogWarning("Update Inquilino - Inquilino with {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Inquilino not found.";

                return NotFound(responseApi);
            }

            await _inquilinoRepository.UpdateInquilino(id, model.ConvertInquilinoUpdateModelToInquilinoEntity());
            await ClearCacheAsync();

            _logger.LogInformation("Update Inquilino - Inquilino updated.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Update Inquilino - Error. Errors {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "Internal server error.";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }


    // DELETE: api/InquilinoController/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> MarkDelete(int id)
    {
        _logger.LogInformation("Delete Inquilino - Start.");
        var responseApi = new ResponseAPI<int>();

        try
        {
            if (!await _inquilinoRepository.Exists(cd => cd.IdInquilino == id))
            {
                _logger.LogWarning("Delete Inquilino - Inquilino with {id} not found.", id);
                responseApi.Success = false;
                responseApi.Message = "Inquilino not found.";
                return NotFound(responseApi);
            }

            await _inquilinoRepository.MarkDeleted(id);
            await ClearCacheAsync();

            _logger.LogInformation("Delete Inquilino - Inquilino deleted.");
            responseApi.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Delete Inquilino - Error. Errors {ex}", ex);
            responseApi.Success = false;
            responseApi.Message = "internal server error";
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    private async Task ClearCacheAsync()
    {
        await _outputCacheStore.EvictByTagAsync("InquilinosCache", default);
        await _outputCacheStore.EvictByTagAsync("PagosCache", default);
    }

    #region Fields

    private readonly IInquilinoRepository _inquilinoRepository;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly ILogger<InquilinoController> _logger;

    public InquilinoController(IInquilinoRepository inquilinoRepository,
        IOutputCacheStore outputCacheStore, ILogger<InquilinoController> logger)
    {
        _inquilinoRepository = inquilinoRepository;
        _outputCacheStore = outputCacheStore;
        _logger = logger;
    }

    #endregion
}