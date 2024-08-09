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
///     Clase Predeterminada de Pago;
///     Y recupera la info de Inquilino(Es nesesario para funcionar).
/// </summary>
public class PagoRepository : BaseRepository<Pago>, IPagoRepository
{
    public override async Task<Pago> GetById(int id)
    {
        ArgumentNullException.ThrowIfNull(id, "El id no puede ser null");

        if (!await base.Exists(cd => cd.IdPago == id))
            throw new PagoException("El inquilino no puede ser null");

        return await base.GetById(id);
    }

    public async Task<List<PagoInquilinoModel>> GetPago()
    {
        return await _context.Pagos
            .Where(pa => pa.Deleted == false)
            .Join(_context.Inquilinos,
                pa => pa.IdInquilino, inq => inq.IdInquilino,
                (pa, inq) => pa.ConvertPagoEntityToPagoInquilinoModel(inq))
            .ToListAsync();
    }

    public void DetachEntity(Pago entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }

    public async Task MarkRetrasado(int id)
    {
        var pago = await _context.Pagos.FirstOrDefaultAsync(pa => pa.IdPago == id);

        if (pago == null)
            throw new PagoException("El Pago no Existe.");

        pago.Retrasado = false;
        pago.Email = false;
        
        await Update(pago);
    }

    public void CheckRetraso(Pago pago)
    {
        var pagoFechaPagoInDays = pago.FechaPagoInDays;

        if (!pagoFechaPagoInDays.HasValue)
            throw new InvalidOperationException("FechaPagoInDays must have a value.");

        var currentDate = DateTime.Now;

        if ((currentDate.Month == 2 && currentDate.Day == 29 && pagoFechaPagoInDays == 30) ||
            (currentDate.Day == pagoFechaPagoInDays))
            pago.Retrasado = true;
        
    }

    public async Task<List<Pago>> GetRetrasosWithoutEmail()
    {
        return await _context.Pagos
            .Where(pa => pa.Retrasado == true && pa.Email == false && pa.Deleted == false)
            .Include(pa => pa.Inquilino)
            .ToListAsync();
    }

    #region Context

    private readonly DepartContext _context;

    public PagoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}