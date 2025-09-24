using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IInteresadoRepository : IBaseRepository<Interesado>
{
    Task<List<InteresadoModel>> GetByType(string type);
    
    Task<List<Interesado>> GetPendingInteresado();
    
    Task<(bool Success, string Message)> Save(InteresadoDto interesadoDto);
    
    Task MarkDeleted(int id);
}