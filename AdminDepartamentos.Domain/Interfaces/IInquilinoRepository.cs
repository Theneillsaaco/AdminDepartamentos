using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Domain.Services;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IInquilinoRepository : IBaseRepository<Inquilino>
{
    Task<List<InquilinoModel>> GetInquilinos();

    Task<(bool Success, string Message)> Save(Inquilino inquilino);

    Task MarkDeleted(int id);
}