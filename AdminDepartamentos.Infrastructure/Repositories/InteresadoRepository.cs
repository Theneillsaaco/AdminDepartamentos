using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Extentions;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Mapping;
using AdminDepartamentos.Infrastructure.Models.InteresadoModels;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

public class InteresadoRepository : BaseRepository<InteresadoEntity>, IInteresadoRepository
{
    public async Task<List<InteresadoModel>> GetByType(string type, int? lastId = null, int take = 20)
    {
        var query = _context.Interesados
            .AsNoTracking()
            .Where(inte => inte.TipoUnidadHabitacional == type);

        if (lastId.HasValue)
            query = query.Where(inte => inte.IdInteresado < lastId.Value);
        
        return await query
            .OrderBy(p => p.IdInteresado)
            .Select(inte => inte.ConvertInteresadoEntityToInteresadoModel())
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<InteresadoEntity>> GetPendingInteresado(int? lastId = null, int take = 20)
    {
        var query = _context.Interesados
            .AsNoTracking();

        if (lastId.HasValue)
            query = query.Where(inte => inte.IdInteresado > lastId.Value);

        return await query
            .Take(take)
            .OrderBy(inte => inte.Fecha)
            .ToListAsync();
    }

    public override async Task<InteresadoEntity> GetById(int id)
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
            var tipoDomain = ParseTipo(interesadoDto.TipoUnidadHabitacional);
            
            var result = Interesado.create(
                interesadoDto.FirstName,
                interesadoDto.LastName,
                interesadoDto.Telefono,
                tipoDomain
            );
            
            if (result.IsError)
                throw new InteresadoExceptions(result.ErrorValue.ToString());
            
            var domain = result.ResultValue;
            var entity = domain.ToEntity();
            
            await _context.Interesados.AddAsync(entity);
            await _context.SaveChangesAsync();
            return (true, "Guardado el Interesado correctamente.");
        }
        catch (Exception ex)
        {
            return (false, $"Ocurrio un error al crear el interesado. Error: {ex.Message}");
        }
    }

    public async Task UpdateInteresado(int id, string firstName, string lastName, string telefono, string tipoUnidadHabitacional)
    {
        var entity = await GetById(id);
        
        if (entity is null)
            throw new InteresadoExceptions("El interesado no Existe.");

        var domain = entity.ToDomain();
        var tipoDomain = ParseTipo(tipoUnidadHabitacional);
        
        var updated = Interesado.update(
            firstName, 
            lastName, 
            telefono,
            tipoDomain,
            domain
        );
        
        if (updated.IsError)
            throw new InteresadoExceptions(updated.ErrorValue.ToString());
        
        entity.Apply(updated.ResultValue);
        await _context.SaveChangesAsync();
    }

    public async Task MarkDeleted(int id)
    {
        var entity = await GetById(id);

        if (entity is null)
            throw new InteresadoExceptions("El interesado no Existe.");

        var domain = entity.ToDomain();
        var deleted = Interesado.markDeleted(domain);
        
        entity.Apply(deleted);
        await _context.SaveChangesAsync();
    }
    
    private static TipoUnidad ParseTipo(string tipo)
    {
        return tipo switch
        {
            "Apartamento" => TipoUnidad.Apartamento,
            "Local" => TipoUnidad.Local,
            _ => throw new UnidadHabitacionalException("Tipo de unidad habitacional invalido")
        };
    }

    #region Fields

    private readonly DepartContext _context;

    public InteresadoRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}