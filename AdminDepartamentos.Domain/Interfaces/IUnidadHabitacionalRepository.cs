using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IUnidadHabitacionalRepository : IBaseRepository<UnidadHabitacional>
{
    Task<List<UnidadHabitacionalModel>> GetUnidadHabitacionales();
    
    Task<List<UnidadHabitacional>> GetAvailableUnidadHabitacional();
    
    Task<List<UnidadHabitacional>> GetOccupiedUnidadHabitacional();
    
    Task<(bool Success, string Message)> Save(UnidadHabitacionalDto unidadHabitacionalDto);
    
    Task<bool> AssignInquilino(int idUnidadHabitacional, int idInquilino);

    Task<bool> ReleaseUnit(int idUnidadHabitacional);
    
    Task MarkDeleted(int id);
}