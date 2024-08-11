using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AdminDepartamentos.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InquilinoController : ControllerBase
{
    private readonly IInquilinoRepository _inquilinoRepository;
    private readonly IOutputCacheStore _outputCacheStore;

    public InquilinoController(IInquilinoRepository inquilinoRepository, IOutputCacheStore outputCacheStore)
    {
        _inquilinoRepository = inquilinoRepository;
        _outputCacheStore = outputCacheStore;
    }

    // GET: api/<InquilinoController>
    [HttpGet]
    [Route("GetAll")]
    [OutputCache(PolicyName = "InquilinosCache")]
    public async Task<IActionResult> GetAll()
    {
        var responseApi = new ResponseAPI<List<InquilinoViewModel>>();

        try
        {
            var inquilinos = await _inquilinoRepository.GetInquilinos();

            var inquilinoViewModels =
                inquilinos.Select(inq => inq.ConvertInquilinoViewModelToInquilinoModel()).ToList();

            responseApi.Success = true;
            responseApi.Data = inquilinoViewModels;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // GET api/<InquilinoController>/5
    [HttpGet]
    [Route("GetById/{id}")]
    [OutputCache(PolicyName = "InquilinosCache")]
    public async Task<IActionResult> GetById(int id)
    {
        var responseApi = new ResponseAPI<Inquilino>();

        try
        {
            var inquilino = await _inquilinoRepository.GetById(id);
            
            if (inquilino is null)
            {
                responseApi.Success = false;
                responseApi.Message = $"Inquilino with Id {id} not found.";
                return NotFound(responseApi);
            }
            
            responseApi.Success = true;
            responseApi.Data = inquilino;
        }
        catch (Exception ex)
        {
            responseApi.Success = false;
            responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError, responseApi);
        }

        return Ok(responseApi);
    }

    // POST api/<InquilinoController>
    [HttpPost]
    [Route("Save")]
    public async Task<IActionResult> Save([FromBody] InquilinoSaveModel inquilinoSaveModel)
    {
        var responseApi = new ResponseAPI<InquilinoSaveModel>();

        if (inquilinoSaveModel.InquilinoDto is null || inquilinoSaveModel.PagoDto is null)
        {
            responseApi.Success = false;
            responseApi.Message = "Datos invalidos.";
            return BadRequest(ModelState);
        }

        var result = await _inquilinoRepository.Save(inquilinoSaveModel.InquilinoDto, inquilinoSaveModel.PagoDto);
        await ClearCacheAsync();
        
        responseApi.Success = result.Success;
        responseApi.Message = result.Message;

        return Ok(responseApi);
    }

    // PUT api/<InquilinoController>/5
    [HttpPut]
    [Route("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InquilinoUpdateModel inquilinoUpdateModel)
    {
        var responseApi = new ResponseAPI<InquilinoUpdateModel>();

        try
        {
            if (!await _inquilinoRepository.Exists(cd => cd.IdInquilino == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Inquilino with Id {id} not found.";

                return NotFound(responseApi);
            }

            var inquilino = inquilinoUpdateModel.ConvertEntityInquilinoToInquilinoUpdateModel();
            inquilino.IdInquilino = id;

            await _inquilinoRepository.Update(inquilino);
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


    // DELETE api/<InquilinoController>/5
    [HttpPatch]
    [Route("Delete/{id}")]
    public async Task<IActionResult> MarkDelete(int id)
    {
        var responseApi = new ResponseAPI<InquilinoDeletedModel>();

        try
        {
            if (!await _inquilinoRepository.Exists(cd => cd.IdInquilino == id))
            {
                responseApi.Success = false;
                responseApi.Message = $"Inquilino with Id {id} not found.";
                return NotFound(responseApi);
            }
            
            await _inquilinoRepository.MarkDeleted(id);
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
        await _outputCacheStore.EvictByTagAsync("InquilinosCache", default);
        await _outputCacheStore.EvictByTagAsync("PagosCache", default);
    }
}