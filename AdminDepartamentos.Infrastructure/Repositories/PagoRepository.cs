using AdminDepartament.Infrastructure.Context;
using AdminDepartament.Infrastructure.Core;
using AdminDepartament.Infrastructure.Exceptions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartament.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartament.Infrastructure.Repositories;

/// <summary>
/// Clase Predeterminada de Pago;
/// Y recupera la info de Inquilino(Es nesesario para funcionar).
/// </summary>
public class PagoRepository : BaseRepository<Pago>, IPagoRepository
{
    #region Context
    
    private readonly DepartContext _context;
        
    public PagoRepository(DepartContext context) : base(context)
    {
        this._context = context;
    }
    
    #endregion

    public override async Task<Pago> GetById(int id)
    {
        ArgumentNullException.ThrowIfNull(id, "El id no puede ser null");

        if (!await base.Exists(cd => cd.IdPago == id))
            throw new PagoException("El inquilino no puede ser null");
        
        return await base.GetById(id);
    }

    public async Task<List<PagoInquilinoModel>> GetPago()
    {
        var pago = await _context.Pagos
            .Where(co => co.Deleted == false)
            .Join(_context.Inquilinos,
                co => co.IdInquilino, inq => inq.IdInquilino,
                (co, inq) => co.ConvertPagoEntityToPagoInquilinoModel(inq))
            .ToListAsync();
        
        return pago;
    }
    
    public void DetachEntity(Pago entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }
    
    public async Task MarkRetrasado(int id)
    {
        var pago = new Pago();

        if (!await base.Exists(cd => cd.IdPago == pago.IdPago))
            throw new InquilinoException("El Pago no Existe.");

        pago.Retrasado = false;

        await Update(pago);
    }
    
    public void CheckRetraso(Pago pago)
    {
        int? pagoFechaPagoInDays = pago.FechaPagoInDays;

        if (!pagoFechaPagoInDays.HasValue)
            throw new InvalidOperationException("FechaPagoInDays must have a value.");
        
        DateTime currentDate = DateTime.Now;
        
        if (currentDate.Month == 2 && currentDate.Day == 29 && pagoFechaPagoInDays == 30)
        {
            pago.Retrasado = true;
        }
        else if (currentDate.Day == pagoFechaPagoInDays)
        {
            pago.Retrasado = true;
        }
    }
}