using AdminDepartamentos.Infrastructure.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

/// <summary>
///     Clase Predeterminada de Inquilino;
///     GetAll, GetById, Save, ...
/// </summary>
public class InquilinoRepository : BaseRepository<Inquilino>, IInquilinoRepository
{
    public override async Task<Inquilino> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El Id no puede ser menor o igual a cero.", nameof(id));

        if (!await base.Exists(cd => cd.IdInquilino == id))
            throw new InquilinoException("El inquilino no existe.");

        return await base.GetById(id);
    }

    public async Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto)
    {
        if (inquilinoDto is null) throw new ArgumentNullException(nameof(inquilinoDto), "El Inquilino no puede ser null.");
        if (pagoDto is null) throw new ArgumentNullException(nameof(pagoDto), "El Pago no puede ser null.");

        using var transaction = await _context.Database.BeginTransactionAsync();
        {
            try
            {
                if (await base.Exists(cd => cd.Cedula == inquilinoDto.Cedula))
                    throw new InquilinoException("El inquilino ya Existe.");

                var newInquilino = inquilinoDto.ConvertEntityInquilinoToInquilinoDto();

                await _context.Inquilinos.AddAsync(newInquilino);
                await _context.SaveChangesAsync();

                var newPago = new Pago
                {
                    IdInquilino = newInquilino.IdInquilino,
                    Monto = pagoDto.Monto,
                    NumDeposito = pagoDto.NumDeposito,
                    FechaPagoInDays = pagoDto.FechaPagoInDays
                };

                await _context.Pagos.AddAsync(newPago);
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
        if (entity == null) throw new ArgumentNullException(nameof(entity), "El Inquilino no puede ser null.");

        await base.Update(entity);
    }

    public async Task<List<InquilinoModel>> GetInquilinos()
    {
        return await _context.Inquilinos
            .Where(inq => !inq.Deleted)
            .Select(inq => inq.ConvertInquilinoEntityToInquilinoModel())
            .ToListAsync();
    }

    public async Task MarkDeleted(int id)
    {
        var inquilino = await _context.Inquilinos.FirstOrDefaultAsync(i => i.IdInquilino == id);

        if (inquilino is null)
            throw new InquilinoException("El inquilino no Existe.");
        
        inquilino.Deleted = true;
        inquilino.DeletedDate = DateTime.Now;
        inquilino.ModifyDate = DateTime.Now;
        
        var pagos = await _context.Pagos.Where(pa => pa.IdInquilino == id).ToListAsync();
        foreach (var pago in pagos)
        {
            pago.Deleted = true;
        }
        
        await _context.SaveChangesAsync();
    }

    #region context

    private readonly DepartContext _context;

    public InquilinoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}