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
            var responseApi = new ResponseAPI<List<PagoViewModel>>();

            try
            {
                var pago = await _pagoRepository.GetPago();

                var pagoViewModels = pago.Select(pa => pa.ConvertToPagoModel()).ToList();

                responseApi.Success = true;
                responseApi.Data = pagoViewModels;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpGet]
        [Route("GetPagoByInquilino{id}")]
        public async Task<IActionResult> GetPagoByInquilino(int id)
        {
            var responseApi = new ResponseAPI<List<PagoViewModel>>();

            try
            {
                var pago = await _pagoRepository.GetPagoByInquilino(id);

                var pagoViewModels = pago.Select(pa => pa.ConvertToPagoModel()).ToList();

                responseApi.Success = true;
                responseApi.Data = pagoViewModels;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return Ok(responseApi);
        }
        
        // GET api/<PagoController>/5
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var responseApi = new ResponseAPI<Pago>();

            try
            {
                var pago = _pagoRepository.GetById(id);

                responseApi.Success = true;
                responseApi.Data = await pago;
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

                    return BadRequest(responseApi);
                }

                var pago = pagoUpdateModel.ConverToPagoEntityToPagoUpdateModel();

                await _pagoRepository.Update(pago);

                responseApi.Success = true;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return Ok(responseApi);
        }
    }
}
