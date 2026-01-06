using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Extentions;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Mapping;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

public class UnidadHabitacionalRepository : BaseRepository<UnidadHabitacionalEntity>, IUnidadHabitacionalRepository
{
    public async Task<List<UnidadHabitacionalModel>> GetUnidadHabitacionales()
    {
        var unidades = await _context.UnidadHabitacionals
            .Include(uni => uni.InquilinoActual)
            .OrderBy(uni => uni.IdUnidadHabitacional)
            .ToListAsync();

        var interesados = await _context.Interesados
            .Select(i => i.ConvertInteresadoEntityToInteresadoModel())
            .ToListAsync();

        return unidades.Select(uni => new UnidadHabitacionalModel
        {
            IdUnidadHabitacional = uni.IdUnidadHabitacional,
            Name = uni.Name,
            Tipo = uni.Tipo,
            LightCode = uni.LightCode,
            Occupied = uni.IdInquilinoActual != null,
            InquilinoActual = uni.InquilinoActual != null
                ? uni.InquilinoActual.ConvertInquilinoEntityToInquilinoModel()
                : null,
            Interesados = interesados
                .Where(i => i.TipoUnidadHabitacional == uni.Tipo)
                .ToList()
        }).ToList();
    }


    public async Task<List<UnidadHabitacionalEntity>> GetAvailableUnidadHabitacional()
    {
        return await _context.UnidadHabitacionals
            .Include(uni => uni.Interesados)
            .Where(uni => uni.IdInquilinoActual == null)
            .ToListAsync();
    }

    public async Task<List<UnidadHabitacionalEntity>> GetOccupiedUnidadHabitacional()
    {
        return await _context.UnidadHabitacionals
            .Include(uni => uni.InquilinoActual)
            .Where(uni => uni.IdInquilinoActual != null)
            .ToListAsync();
    }

    public override async Task<UnidadHabitacionalEntity> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El Id no puede ser menor o igual a cero.", nameof(id));

        if (!await base.Exists(cd => cd.IdUnidadHabitacional == id))
            throw new InquilinoException("La Unidad Habitacional no existe.");

        return await base.GetById(id);
    }

    public async Task<(bool Success, string Message)> Save(UnidadHabitacionalDto unidadHabitacionalDto)
    {
        if (unidadHabitacionalDto is null)
            return (false, "La unidad Habitacional no puede ser null");

        try
        {
            var tipoDomain = ParseTipo(unidadHabitacionalDto.Tipo);
            
            var domainResult = UnidadHabitacionalModule.create(unidadHabitacionalDto.Name, tipoDomain, unidadHabitacionalDto.LightCode);
            
            if (domainResult.IsError)
                throw new UnidadHabitacionalException(domainResult.ErrorValue.ToString());

            var enity = domainResult.ResultValue.ToEntity();
            await _context.UnidadHabitacionals.AddAsync(enity);
            await _context.SaveChangesAsync();
            return (true, "Unidad Habitacional creada exitosamente.");
        }
        catch (Exception ex)
        {
            return (false, "Ocurrio un error al crear la Unidad Habitacional.");
        }
    }

    public async Task UpdateUnidadHabitacional(int id, string name, string tipo, string lightCode)
    {
        var entity = await GetById(id);
        var domain = entity.ToDomain();
        var tipoDomain = ParseTipo(tipo);
        
        var updated = UnidadHabitacionalModule.updateInfo(
            name, tipoDomain, lightCode, domain
        );
        
        if (updated.IsError)
            throw new UnidadHabitacionalException(updated.ErrorValue.ToString());
        
        entity.Apply(updated.ResultValue);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AssignInquilino(int idUnidadHabitacional, int idInquilino)
    {
        var entity = await GetById(idUnidadHabitacional);
        var domain = entity.ToDomain();

        var updated = UnidadHabitacionalModule.assingInquilino(idInquilino, domain);
        
        if (updated.IsError)
            throw new UnidadHabitacionalException(updated.ErrorValue.ToString());

        entity.Apply(updated.ResultValue);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReleaseUnit(int idUnidadHabitacional)
    {
        var entity = await GetById(idUnidadHabitacional);
        var domain = entity.ToDomain();
        
        var released = UnidadHabitacionalModule.release(domain);
        
        entity.Apply(released);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task MarkDeleted(int id)
    {
        var entity = await GetById(id);
        var domain = entity.ToDomain();
        
        var deleted= UnidadHabitacionalModule.markDeleted(domain);
        
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

    public UnidadHabitacionalRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}