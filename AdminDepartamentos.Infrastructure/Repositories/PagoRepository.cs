using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Extentions;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

/// <summary>
///     Clase Predeterminada de Pago;
///     Y recupera la info de Inquilino(Es nesesario para funcionar).
/// </summary>
public class PagoRepository : BaseRepository<Pago>, IPagoRepository
{
    public override async Task<Pago> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El id no puede ser null.", nameof(id));

        if (!await base.Exists(pa => pa.IdPago == id))
            throw new PagoException("El Pago no existe.");

        return await base.GetById(id);
    }

    public async Task<List<PagoInquilinoModel>> GetPago()
    {
        return await _context.Pagos
            .Include(p => p.Inquilino)
            .Select(p => p.ConvertPagoEntityToPagoInquilinoModel(p.Inquilino))
            .ToListAsync();
    }

    public void CheckRetraso(Pago pago)
    {
        var currentDate = DateTime.Now;
        
        if ((currentDate is { Month: 2, Day: 29 } && pago.FechaPagoInDays == 30) ||
            (currentDate.Day == pago.FechaPagoInDays))
            pago.Retrasado = true;
    }

    public async Task<List<Pago>> GetRetrasosWithoutEmail()
    {
        return await _context.Pagos
            .Where(pa => pa.Retrasado && !pa.Email)
            .Include(pa => pa.Inquilino)
            .ToListAsync();
    }

    #region Fields

    private readonly DepartContext _context;

    public PagoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}