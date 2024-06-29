using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IInquilinoRepository : IBaseRepository<Inquilino>
{
    Task<List<InquilinoModel>> GetInquilinos();

    Task<InquilinoPagoModel> Save(InquilinoPagoModel model);
    
    Task<Inquilino> GetByNumDepartamento(int numDepart);

    Task MarkDeleted(int id);
}