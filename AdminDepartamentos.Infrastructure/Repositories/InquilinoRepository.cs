using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Extentions;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Domain.Services;
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
    public async Task<List<InquilinoModel>> GetInquilinos()
    {
        return await _context.Inquilinos
            .Select(inq => inq.ConvertInquilinoEntityToInquilinoModel())
            .ToListAsync();
    }

    public override async Task<Inquilino> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El Id no puede ser menor o igual a cero.", nameof(id));

        if (!await base.Exists(cd => cd.IdInquilino == id))
            throw new InquilinoException("El inquilino no existe.");

        return await base.GetById(id);
    }

    public async Task<(bool Success, string Message)> Save(Inquilino inquilino)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (await base.Exists(cd => cd.Cedula == inquilino.Cedula))
                throw new InquilinoException("El inquilino ya existe. Por favor, use otro número de cédula.");

            await _context.Inquilinos.AddAsync(inquilino);
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

    public override async Task Update(Inquilino entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "El Inquilino no puede ser null.");

        await base.Update(entity);
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
    private readonly InquilinoService _domainService;

    public InquilinoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}