using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.PagoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDepartamentos.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IPagoRepository _pagoRepository;

        public PagoController(IPagoRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }
 
        // GET: api/<PagoController>
        [HttpGet]
        [Route("GetPago")]
        public async Task<IActionResult> GetPago()
        {
            var responseApi = new ResponseAPI<List<PagoGetByInquilinoModel>>();

            try
            {
                var pago = await _pagoRepository.GetPago();

                var pagoGetModel = pago.Select(pa => pa.ConvertToPagoGetByInquilinoModel()).ToList();

                responseApi.Success = true;
                responseApi.Data = pagoGetModel;
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
        public async Task<IActionResult> GetById(int id)
        {
            var responseApi = new ResponseAPI<PagoGetModel>();

            try
            {
                var pago = await _pagoRepository.GetById(id);

                var pagoGetModel = pago.ConvertToPagoGetModel();
                
                responseApi.Success = true;
                responseApi.Data = pagoGetModel;
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
                if (!await _pagoRepository.Exists(cd => cd.IdPago == id))
                {
                    responseApi.Success = false;
                    responseApi.Message = $"Pago with Id {id} not found.";

                    return NotFound(responseApi);
                }
                
                var getPago = await _pagoRepository.GetById(id);
                
                _pagoRepository.DetachEntity(getPago);
                
                var pago = pagoUpdateModel.ConverToPagoEntityToPagoUpdateModel();
                pago.IdPago = id;
                pago.IdInquilino = getPago.IdInquilino;
                
                await _pagoRepository.Update(pago);

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

        [HttpPut]
        [Route("Retrasado/{id}")]
        public async Task<IActionResult> MarkRetrasado(int id)
        {
            var responseApi = new ResponseAPI<PagoDeletedModel>();

            try
            {
                var pago = _pagoRepository.MarkRetrasado(id);

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
    }
}
