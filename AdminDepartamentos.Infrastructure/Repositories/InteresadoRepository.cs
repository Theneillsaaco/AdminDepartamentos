using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Extentions;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

public class InteresadoRepository : BaseRepository<Interesado> , IInteresadoRepository
{
    public async Task<List<InteresadoModel>> GetByType(string type)
    {
        return await _context.Interesados
            .Where(inte => inte.TipoUnidadHabitacional == type && !inte.Deleted)
            .Select(inte => inte.ConvertInteresadoEntityToInteresadoModel())
            .ToListAsync();
    }

    public async Task<List<Interesado>> GetPendingInteresado()
    {
        return await _context.Interesados
            .Where(inte => inte.Deleted)
            .OrderBy(inte => inte.Fecha)
            .ToListAsync();
    }

    public override async Task<Interesado> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El Id no puede ser menor o igual a cero.", nameof(id));
        
        if (!await base.Exists(cd => cd.IdInteresado == id))
            throw new InquilinoException("El Interesado no existe.");
        
        return await base.GetById(id);
    }

    public async Task<(bool Success, string Message)> Save(InteresadoDto interesadoDto)
    {
        if (interesadoDto is null)
            return (false, "El interesado no puede ser null");
        
        try
        {
            await _context.Interesados.AddAsync(interesadoDto.ConvertInteresadoDtoToInteresadoEntity());
            await _context.SaveChangesAsync();
            return (true, "Guardado el Interesado correctamente.");
        }
        catch (Exception ex)
        {
            return (false, $"Ocurrio un error al crear el interesado. Error: {ex.Message}");
        }
    }
    
    public override async Task Update(Interesado entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "El Interesado no puede ser null.");
        
        await base.Update(entity);
    }

    public async Task MarkDeleted(int id)
    {
        var interesado = await GetById(id);
        
        if (interesado is null)
            throw new InteresadoExceptions("El interesado no Existe.");
        
        interesado.MarkDeleted();
        await _context.SaveChangesAsync();
    }

    #region Fields

    private readonly DepartContext _context;
    
    public InteresadoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }
    
    #endregion
}