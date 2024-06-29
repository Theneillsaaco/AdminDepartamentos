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
/// Clase Predeterminada de Inquilino; GetAll, GetById, Save, ...
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

    public async Task<InquilinoPagoModel> Save(InquilinoPagoModel model)
    {
        ArgumentNullException.ThrowIfNull(model, "El modelo no puede ser null.");
        ArgumentNullException.ThrowIfNull(model.Inquilino, "El Iqnuilino no puede ser null.");
        ArgumentNullException.ThrowIfNull(model.Pago, "El pago no puede ser null.");
        
        // Comenzar una transaccion para asegurar la atomicidad
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Agregar el inquilino
            await _context.Inquilinos.AddAsync(model.Inquilino);
            await _context.SaveChangesAsync();
            
            // Asignar el id del Inquilino al pago
            model.Pago.IdInquilino = model.Inquilino.IdInquilino;
            
            // Agregar el pago
            var idPago = await AddPagoAsync(model.Pago);

            model.Pago.IdPago = idPago;

            await transaction.CommitAsync();

            return model;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<int> AddPagoAsync(Pago pago)
    {
        // Insertar el pago
        _context.Pagos.Add(pago);
        await _context.SaveChangesAsync();

        // Recuperar el ID insertado (asumiendo que IdPago es la clave primaria)
        var insertedPago = await _context.Pagos
            .OrderByDescending(p => p.IdPago)
            .FirstOrDefaultAsync(p => p.FechaPago == pago.FechaPago && p.IdInquilino == pago.IdInquilino);

        return insertedPago?.IdPago ?? 0;
    }

    public async Task<Inquilino> GetByNumDepartamento(int numDepart)
    {
        ArgumentNullException.ThrowIfNull(numDepart, "El Id no puede ser null.");

        if (!await base.Exists(cd => cd.NumDepartamento == numDepart))
            throw new InquilinoException("El inquilino no existe.");

        return await _context.FindAsync<Inquilino>(GetIdByNumDepartamento(numDepart));
    }
    
    private async Task<int> GetIdByNumDepartamento(int numDepart)
    {
        var inquilino = await _context.Inquilinos.FirstOrDefaultAsync(i => i.NumDepartamento == numDepart);
        if (inquilino == null)
        {
            throw new InquilinoException("No se encontró numDepart asociado al número de departamento.");
        }
        
        return inquilino.IdInquilino;
    }
    
    public override async Task Update(Inquilino entity)
    {
        ArgumentNullException.ThrowIfNull(entity, "El Inquilino no puede ser null.");

        if (!await base.Exists(cd => cd.IdInquilino != entity.IdInquilino))
            throw new InquilinoException("El inquilino no Existe.");

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