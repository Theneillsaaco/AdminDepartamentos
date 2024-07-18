using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IInquilinoRepository : IBaseRepository<Inquilino>
{
    Task<List<InquilinoModel>> GetInquilinos();

    Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto);

    Task MarkDeleted(int id);
}