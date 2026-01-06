using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core.Interfaces;
using AdminDepartamentos.Infrastructure.Models.InteresadoModels;

namespace AdminDepartamentos.Infrastructure.Interfaces;

public interface IInteresadoRepository : IBaseRepository<InteresadoEntity>
{
    Task<List<InteresadoModel>> GetByType(string type);

    Task<List<InteresadoEntity>> GetPendingInteresado();

    Task<(bool Success, string Message)> Save(InteresadoDto interesadoDto);

    Task UpdateInteresado(int id, string firstName, string lastName, string telefono, string tipoUnidadHabitacional);
    
    Task MarkDeleted(int id);
}