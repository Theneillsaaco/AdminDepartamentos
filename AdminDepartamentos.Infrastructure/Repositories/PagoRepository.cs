using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Extentions;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Mapping;
using AdminDepartamentos.Infrastructure.Models.PagoModels;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

/// <summary>
///     Clase Predeterminada de Pago;
///     Y recupera la info de Inquilino(Es nesesario para funcionar).
/// </summary>
public class PagoRepository : BaseRepository<PagoEntity>, IPagoRepository
{
    public override async Task<PagoEntity> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id no puede ser null.", nameof(id));

        if (!await base.Exists(pa => pa.IdPago == id))
            throw new PagoException("El Pago no existe.");

        return await base.GetById(id);
    }

    public async Task<List<PagoInquilinoModel>> GetPago(int? lastId = null, int take = 20)
    {
        var query = _context.Pagos
            .AsNoTracking()
            .Include(p => p.Inquilino)
            .OrderBy(p => p.IdPago)
            .AsQueryable();
        
        if (lastId.HasValue)
            query = query.Where(p => p.IdPago > lastId.Value);

        return await query
            .Take(take)
            .Select(p => p.ToPagoInquilinoModel(p.Inquilino))
            .ToListAsync();
    }

    public async Task<List<PagoEntity>> GetRetrasosWithoutEmail()
    {
        return await _context.Pagos
            .AsNoTracking()
            .Where(pa => pa.Retrasado && !pa.Email)
            .Include(pa => pa.Inquilino)
            .ToListAsync();
    }

    public async Task<List<PagoEntity>> GetPagosForRetraso()
    {
        return await _context.Pagos.ToListAsync();
    }

    public async Task<bool> UpdatePago(int id, PagoEntity pago)
    {
        var entity = await _context.Pagos.FirstOrDefaultAsync(p => p.IdPago == id);
        
        if (entity is null)
            return false;

        var domain = entity.ToDomain();
        
        var fechaResult = FechaPagoModule.create(pago.FechaPagoInDays);
        if (fechaResult.IsError)
            throw new Exception("Fecha del pago es invalida");

        var updatedResult = PagoModule.update(
            pago.NumDeposito,
            pago.Monto,
            fechaResult.ResultValue,
            domain
        );
        
        if (updatedResult.IsError)
            throw new Exception("Error al actualizar el pago");
        
        entity.Apply(updatedResult.ResultValue);
        
        await _context.SaveChangesAsync();
        return true;
    }

    #region Fields

    private readonly DepartContext _context;

    public PagoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}