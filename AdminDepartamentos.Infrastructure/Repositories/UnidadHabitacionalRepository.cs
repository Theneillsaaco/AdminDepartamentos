using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Extentions;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Core;
using AdminDepartamentos.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Repositories;

public class UnidadHabitacionalRepository : BaseRepository<UnidadHabitacional>, IUnidadHabitacionalRepository
{
    public async Task<List<UnidadHabitacionalModel>> GetUnidadHabitacionales()
    {
        return await _context.UnidadHabitacionals
            .Include(uni => uni.InquilinoActual)
            .Include(uni => uni.Interesados)
            .Select(uni => new UnidadHabitacionalModel
            {
                IdUnidadHabitacional = uni.IdUnidadHabitacional,
                Name = uni.Name,
                Tipo = uni.Tipo,
                LightCode = uni.LightCode,
                Occupied = uni.IdInquilinoActual != null,
                InquilinoActual = uni.InquilinoActual != null
                    ? uni.InquilinoActual.ConvertInquilinoEntityToInquilinoModel()
                    : null,
                Interesados = uni.Interesados
                    .Select(i => i.ConvertInteresadoEntityToInteresadoModel())
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<List<UnidadHabitacional>> GetAvailableUnidadHabitacional()
    {
        return await _context.UnidadHabitacionals
            .Include(uni => uni.Interesados)
            .Where(uni => uni.IdInquilinoActual == null)
            .ToListAsync();
    }

    public async Task<List<UnidadHabitacional>> GetOccupiedUnidadHabitacional()
    {
        return await _context.UnidadHabitacionals
            .Include(uni => uni.InquilinoActual)
            .Where(uni => uni.IdInquilinoActual != null)
            .ToListAsync();
    }

    public override async Task<UnidadHabitacional> GetById(int id)
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
            await _context.UnidadHabitacionals.AddAsync(unidadHabitacionalDto
                .ConvertUnidadHabitacionalDtoToUnidadHabitacionalEntity());
            await _context.SaveChangesAsync();
            return (true, "Guadado la Unidad Habitacional correctamente.");
        }
        catch (Exception ex)
        {
            return (false, "Ocurrio un error al crear la Unidad Habitacional.");
        }
    }

    public override async Task Update(UnidadHabitacional entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "La Unidad Habitacional no puede ser null.");

        await base.Update(entity);
    }

    public async Task<bool> AssignInquilino(int idUnidadHabitacional, int idInquilino)
    {
        var unidad = await GetById(idUnidadHabitacional);
        unidad.AssignInquilino(idInquilino);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReleaseUnit(int idUnidadHabitacional)
    {
        var unidad = await GetById(idUnidadHabitacional);
        unidad.Release();
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task MarkDeleted(int id)
    {
        var unidad = await GetById(id);

        if (unidad is null)
            throw new UnidadHabitacionalException("La unidad Habitacional no Existe.");

        unidad.MarkDeleted();
        await _context.SaveChangesAsync();
    }

    #region Fields

    private readonly DepartContext _context;

    public UnidadHabitacionalRepository(DepartContext context) : base(context)
    {
        _context = context;
    }

    #endregion
}