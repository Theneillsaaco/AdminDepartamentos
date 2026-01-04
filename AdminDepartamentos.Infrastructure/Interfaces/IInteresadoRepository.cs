using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastucture.Core.Interfaces;

namespace AdminDepartamentos.Infrastucture.Interfaces;

public interface IInteresadoRepository : IBaseRepository<Interesado>
{
    Task<List<InteresadoModel>> GetByType(string type);

    Task<List<Interesado>> GetPendingInteresado();

    Task<(bool Success, string Message)> Save(InteresadoDto interesadoDto);

    Task MarkDeleted(int id);
}