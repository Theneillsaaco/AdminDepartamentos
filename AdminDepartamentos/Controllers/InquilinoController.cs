using AdminDepartamentos.Domain.Context;
using AdminDepartamentos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InquilinoController : ControllerBase
    {
        #region"Context"

        private readonly DepartContext dbContext;

        public InquilinoController(DepartContext context)
        {
            this.dbContext = context;
        }

        #endregion

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var responseApi = new ResponseAPI<List<Inquilino>>();
            var listInquilinos = await dbContext.Inquilinos
                .Where(i => !i.Deleted) //Filter out deleted
                .ToListAsync();

            try
            {
                foreach (var item in await dbContext.Inquilinos.ToListAsync())
                {
                    listInquilinos.Add(new Inquilino
                    {
                        IdInquilino = item.IdInquilino,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        NumDepartamento = item.NumDepartamento,
                        NumTelefono = item.NumTelefono,
                        Cedula = item.Cedula,
                        CreationDate = item.CreationDate,
                        ModifyDate = item.ModifyDate
                    });
                }
                responseApi.Success = true;
                responseApi.Data = listInquilinos;
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpGet]
        [Route("Search/{id}")]
        public async Task<IActionResult> Search(int id)
        {
            var responseApi = new ResponseAPI<Inquilino>();
            var inquilino = new Inquilino();

            try
            {
                var dbInquilino = await dbContext.Inquilinos.FirstOrDefaultAsync(x => x.IdInquilino == id);

                if (dbInquilino is not null)
                {
                    inquilino.IdInquilino = dbInquilino.IdInquilino;
                    inquilino.FirstName = dbInquilino.FirstName;
                    inquilino.LastName = dbInquilino.LastName;
                    inquilino.NumDepartamento = dbInquilino.NumDepartamento;
                    inquilino.NumTelefono = dbInquilino.NumTelefono;
                    inquilino.Cedula = dbInquilino.Cedula;
                    inquilino.CreationDate = dbInquilino.CreationDate;
                    inquilino.ModifyDate = dbInquilino.ModifyDate;

                    responseApi.Success = true;
                    responseApi.Data = inquilino;

                }
                else
                {
                    responseApi.Success = false;
                    responseApi.Message = "No se ha encontrado el inquilino";
                }
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(Inquilino inquilino)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbInquilino = new Inquilino
                {
                    IdInquilino = inquilino.IdInquilino,
                    FirstName = inquilino.FirstName,
                    LastName = inquilino.LastName,
                    NumDepartamento = inquilino.NumDepartamento,
                    NumTelefono = inquilino.NumTelefono,
                    Cedula = inquilino.Cedula,
                    CreationDate = inquilino.CreationDate,
                    Pagos = inquilino.Pagos
                };
     
                dbContext.Inquilinos.Add(dbInquilino);
                await dbContext.SaveChangesAsync();

                if (dbInquilino.IdInquilino != 0)
                {

                    //Registrar pagos
                    if (inquilino.Pagos is not null && inquilino.Pagos.Any())
                    {
                        foreach (var pago in inquilino.Pagos)
                        {
                            pago.IdInquilino = dbInquilino.IdInquilino;
                            dbContext.Pagos.Add(pago);
                        }
                        await dbContext.SaveChangesAsync();
                    }


                    responseApi.Success = true;
                    responseApi.Data = dbInquilino.IdInquilino;
                }
                else
                {
                    responseApi.Success = false;
                    responseApi.Message = "No se ha podido registrar el inquilino o/y pagos";
                }
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }
            
            return Ok(responseApi);
        }

        [HttpPut]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(Inquilino inquilino, int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbInquilino = dbContext.Inquilinos.FirstOrDefault(x => x.IdInquilino == id);

                if (dbInquilino is not null)
                {
                    dbInquilino.FirstName = inquilino.FirstName;
                    dbInquilino.LastName = inquilino.LastName;
                    dbInquilino.NumDepartamento = inquilino.NumDepartamento;
                    dbInquilino.NumTelefono = inquilino.NumTelefono;
                    dbInquilino.Cedula = inquilino.Cedula;
                    dbInquilino.CreationDate = inquilino.CreationDate;
                    dbInquilino.ModifyDate = DateTime.Now;

                    dbContext.Inquilinos.Update(dbInquilino);
                    await dbContext.SaveChangesAsync();

                    responseApi.Success = true;
                    responseApi.Data = dbInquilino.IdInquilino;
                }
                else
                {
                    responseApi.Success = false;
                    responseApi.Message = "No se ha podido registrar el inquilino";
                }
            }
            catch (Exception ex)
            {
                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

        [HttpDelete]
        [Route("Deleted/{id}")]
        public async Task<IActionResult> Deleted(int id)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbInquilino = await dbContext.Inquilinos.FirstOrDefaultAsync(x => x.IdInquilino == id);

                if (dbInquilino is not null)
                {
                    // Eliminar pagos asociados antes de eliminar el inquilino
                    dbInquilino.Deleted = true;

                    foreach(var pago in dbInquilino.Pagos)
                    {
                        pago.Deleted = true;
                    }

                    dbContext.Inquilinos.Update(dbInquilino);
                    await dbContext.SaveChangesAsync();

                    responseApi.Success = true;
                }

                else
                {
                    responseApi.Success = false;
                    responseApi.Message = "No se ha podido encontrar el inquilino";
                }
            }
            catch (Exception ex)
            {

                responseApi.Success = false;
                responseApi.Message = ex.Message;
            }

            return Ok(responseApi);
        }

    }
}
