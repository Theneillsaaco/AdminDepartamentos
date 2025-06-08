using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IInteresadoRepository : IBaseRepository<Interesado>
{
    Task<List<InteresadoModel>> GetByUnidad(int idUnidad);

    Task<List<Interesado>> GetPendingInteresado();
    
    Task<(bool Success, string Message)> Save(InteresadoDto interesadoDto);
    
    Task MarkDeleted(int id);
}