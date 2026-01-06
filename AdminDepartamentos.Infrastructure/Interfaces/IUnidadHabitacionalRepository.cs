using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core.Interfaces;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;

namespace AdminDepartamentos.Infrastructure.Interfaces;

public interface IUnidadHabitacionalRepository : IBaseRepository<UnidadHabitacionalEntity>
{
    Task<List<UnidadHabitacionalModel>> GetUnidadHabitacionales();

    Task<List<UnidadHabitacionalEntity>> GetAvailableUnidadHabitacional();

    Task<List<UnidadHabitacionalEntity>> GetOccupiedUnidadHabitacional();

    Task<(bool Success, string Message)> Save(UnidadHabitacionalDto unidadHabitacionalDto);

    Task UpdateUnidadHabitacional(int id, string name, string tipo, string lightCode);
    
    Task<bool> AssignInquilino(int idUnidadHabitacional, int idInquilino);

    Task<bool> ReleaseUnit(int idUnidadHabitacional);

    Task MarkDeleted(int id);
}