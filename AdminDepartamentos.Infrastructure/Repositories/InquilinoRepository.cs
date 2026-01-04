using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastucture.Context;
using AdminDepartamentos.Infrastucture.Context.Entities;
using AdminDepartamentos.Infrastucture.Core;
using AdminDepartamentos.Infrastucture.Exceptions;
using AdminDepartamentos.Infrastucture.Interfaces;
using AdminDepartamentos.Infrastucture.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastucture.Repositories;

/// <summary>
///     Clase Predeterminada de Inquilino;
///     GetAll, GetById, Save, ...
/// </summary>
public class InquilinoRepository : BaseRepository<InquilinoEntity>, IInquilinoRepository
{
    public async Task<List<InquilinoEntity>> GetInquilinos()
    {
        return await _context.Inquilinos
            .OrderBy(i => i.IdInquilino)
            .ToListAsync();
    }

    public async Task<InquilinoEntity> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El Id no puede ser menor o igual a cero.", nameof(id));

        if (!await base.Exists(cd => cd.IdInquilino == id))
            throw new InquilinoException("El inquilino no existe.");

        return await base.GetById(id);
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