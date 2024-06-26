﻿using AdminDepartament.Infrastructure.Context;
using AdminDepartament.Infrastructure.Core;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartament.Infrastructure.Extentions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartament.Infrastructure.Repositories;

/// <summary>
/// Clase Predeterminada de Pago; Y recupera la info de Inquilino(Es nesesario para funcionar).
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
    

    public async Task<List<PagoInquilinoModel>> GetPago()
    {
        var pago = _context.Pagos
                .Where(co => co.Deleted == false)
                .Join(_context.Inquilinos,
                co => co.IdInquilino, 
                inq => inq.IdInquilino,
                (co, inq) => co.ConvertPagoEntityToPagoInquilinoModel(inq))
                .ToListAsync();

        return await pago;
    }

    public async Task<List<PagoInquilinoModel>> GetPagoByInquilino(int idInquilino)
    {
        var pago = _context.Pagos
                .Where(co => co.Deleted == false 
                         && co.IdInquilino == idInquilino)
                .Join(_context.Inquilinos, 
                co => co.IdInquilino, inq => inq.IdInquilino,
                (co, inq) => co.ConvertPagoEntityToPagoInquilinoModel(inq))
                .ToListAsync();

        return await pago;
    }
}