using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core.Interfaces;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Models.PagoModels;

namespace AdminDepartamentos.Infrastructure.Interfaces;

public interface IInquilinoRepository : IBaseRepository<InquilinoEntity>    
{
    Task<List<InquilinoEntity>> GetInquilinos(int? lastId = null, int take = 20);

    Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto);

    Task UpdateInquilino(int id, InquilinoEntity inquilino);

    Task MarkDeleted(int id);
}