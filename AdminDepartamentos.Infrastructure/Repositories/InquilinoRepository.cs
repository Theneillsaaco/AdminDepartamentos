using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Mapping;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Models.PagoModels;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

/// <summary>
///     Clase Predeterminada de Inquilino;
///     GetAll, GetById, Save, ...
/// </summary>
public class InquilinoRepository : BaseRepository<InquilinoEntity>, IInquilinoRepository
{
    public async Task<List<InquilinoEntity>> GetInquilinos(int? lastId = null, int take = 20)
    {
        var query = _context.Inquilinos
            .AsNoTracking()
            .OrderBy(i => i.IdInquilino)
            .AsQueryable();

        if (lastId.HasValue)
            query = query.Where(i => i.IdInquilino > lastId.Value);

        return await query
            .Take(take)
            .ToListAsync();
    }

    public async Task<InquilinoEntity> GetById(int id)
    {
        if (id <= 0)
            throw new InquilinoException("El Id del inquilino no es válido.");
            
        var entity = await _context.Inquilinos
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.IdInquilino == id);

        if (entity is null)
            throw new InquilinoException("El inquilino no existe.");

        return entity;
    }

    public async Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (await base.Exists(cd => cd.Cedula == inquilinoDto.Cedula))
                throw new InquilinoException("El inquilino ya existe. Por favor, use otro número de cédula.");

            var fechaPago = FechaPagoModule
                .create(pagoDto.FechaPagoInDays)
                .ResultValue;

            var pagoDomain = PagoModule
                .create(
                    pagoDto.NumDeposito,
                    pagoDto.Monto,
                    fechaPago
                ).ResultValue;

            var inquilinoDomain = InquilinoModule
                .create(
                    inquilinoDto.FirstName,
                    inquilinoDto.LastName,
                    inquilinoDto.Cedula,
                    inquilinoDto.NumTelefono,
                    pagoDomain
                ).ResultValue;
            
            var InquilinoEntity = inquilinoDomain.ToEntity();
            
            await _context.Inquilinos.AddAsync(InquilinoEntity);
            await _context.SaveChangesAsync();

            var PagoEntity = pagoDomain.ToEntity(InquilinoEntity.IdInquilino);

            await _context.Pagos.AddAsync(PagoEntity);
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

    public async Task UpdateInquilino(int id, InquilinoEntity inquilino)
    {
        var entity = await _context.Inquilinos
            .Include(i => i.Pago)
            .FirstOrDefaultAsync(i => i.IdInquilino == id);
        
        if (entity is null)
            throw new InquilinoException("El inquilino no existe.");

        var domain = entity.ToDomain();

        var updated = InquilinoModule.update(
            inquilino.FirstName,
            inquilino.LastName,
            inquilino.Cedula,
            inquilino.Telefono,
            domain
        );

        if (updated.IsError)
            throw new Exception("Error al actualizar el inquilino");

        entity.Apply(updated.ResultValue);
        await _context.SaveChangesAsync();
    }

    public async Task MarkDeleted(int id)
    {
        var inquilino = await _context.Inquilinos
            .Include(i => i.Pago)
            .FirstOrDefaultAsync(i => i.IdInquilino == id);

        if (inquilino is null)
            throw new InquilinoException("El inquilino no Existe.");

        inquilino.MarkDeleted();

        await _context.SaveChangesAsync();
    }

    #region Fields

    private readonly DepartContext _context;

    public InquilinoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}