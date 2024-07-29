using AdminDepartament.Infrastructure.Context;
using AdminDepartament.Infrastructure.Core;
using AdminDepartament.Infrastructure.Exceptions;
using AdminDepartament.Infrastructure.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartament.Infrastructure.Repositories;

/// <summary>
/// Clase Predeterminada de Inquilino;
/// GetAll, GetById, Save, ...
/// </summary>
public class InquilinoRepository : BaseRepository<Inquilino>, IInquilinoRepository
{
    #region context
    
    private readonly DepartContext _context;
    
        public InquilinoRepository(DepartContext context) : base(context)
        {
            _context = context;
        }
    
    #endregion

    public override async Task<Inquilino> GetById(int id)
    {
        ArgumentNullException.ThrowIfNull(id, "El Id no puede ser null.");
        
        if (!await base.Exists(cd => cd.IdInquilino == id))
            throw new InquilinoException("El inquilino no existe.");

        return await base.GetById(id);
    }

    public async Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto)
    {
        ArgumentNullException.ThrowIfNull(inquilinoDto, "El Inquilino no puede ser null.");
        ArgumentNullException.ThrowIfNull(pagoDto, "El pago no puede ser null.");

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (await base.Exists(cd => cd.Cedula == inquilinoDto.Cedula))
                    throw new InquilinoException("El inquilino ya Existe.");

                var newInquilino = inquilinoDto.ConvertEntityInquilinoToInquilinoDto();
                
                _context.Inquilinos.Add(newInquilino);
                await _context.SaveChangesAsync();

                var newPago = new Pago
                {
                    IdInquilino = newInquilino.IdInquilino,
                    Monto = pagoDto.Monto,
                    NumDeposito =  pagoDto.NumDeposito
                };
                
                _context.Pagos.Add(newPago);
                await _context.SaveChangesAsync();
                
                await transaction.CommitAsync();

                return (true, "Inquilino y pago creados exitosamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Ocurrio un error al crear el inquilino y el pago. Error: {ex.Message}");
            }
        }
    }
    
    public override async Task Update(Inquilino entity)
    {
        ArgumentNullException.ThrowIfNull(entity, "El Inquilino no puede ser null.");
        
        await base.Update(entity);
    }
    
    public async Task<List<InquilinoModel>> GetInquilinos()
    {
        var inquilino = _context.Inquilinos
            .Where(inq => inq.Deleted == false)
            .Select(inq => inq.ConvertInquilinoEntityToInquilinoModel())
            .ToListAsync();

        return await inquilino;
    }

    public async Task MarkDeleted(int id)
    {
        var inquilino = new Inquilino();
        var pago = new Pago();
        
        if (!await base.Exists(cd => cd.IdInquilino == inquilino.IdInquilino))
            throw new InquilinoException("El inquilino no Existe.");

        inquilino.Deleted = true;
        inquilino.DeletedDate = DateTime.Now;
        inquilino.ModifyDate = DateTime.Now;
        pago.Deleted = true;

        await Update(inquilino);
    }
}