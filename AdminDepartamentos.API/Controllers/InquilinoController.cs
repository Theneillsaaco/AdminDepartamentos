using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminDepartamentos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquilinoController : ControllerBase
    {
        private readonly IInquilinoRepository _inquilinoRepository;

        public InquilinoController(IInquilinoRepository inquilinoRepository)
        {
            _inquilinoRepository = inquilinoRepository;
        }
        
        // GET: api/<InquilinoController>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var responseApi = new ResponseAPI<List<InquilinoViewModel>>();

            try
            {
                var inquilinos = await _inquilinoRepository.GetInquilinos();

                var inquilinoViewModels = inquilinos.Select(inq => inq.ConvertInquilinoViewModelToInquilinoModel()).ToList();

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
        public async Task<IActionResult> GetById(int id)
        {
            var responseApi = new ResponseAPI<Inquilino>();

            try
            {
                var inquilino = _inquilinoRepository.GetById(id);

                responseApi.Success = true;
                responseApi.Data = await inquilino;
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

            responseApi.Success = result.Success;
            responseApi.Message = result.Message;
            
            return Ok(responseApi);
        }

        // PUT api/<InquilinoController>/5
        [HttpPut]
        [Route("Update{id}")]
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
        [HttpPut]
        [Route("Delete{id}")]
        public async Task<IActionResult> MarkDelete(int id)
        {
            var responseApi = new ResponseAPI<InquilinoDeletedModel>();

            try
            {
                var inquilino = _inquilinoRepository.MarkDeleted(id);

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
