using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastucture.Context.Entities;
using AdminDepartamentos.Infrastucture.Core.Interfaces;

namespace AdminDepartamentos.Infrastucture.Interfaces;

public interface IInquilinoRepository : IBaseRepository<InquilinoEntity>    
{
    Task<List<InquilinoEntity>> GetInquilinos();

    Task<(bool Success, string Message)> Save(InquilinoDto inquilinoDto, PagoDto pagoDto);

    Task UpdateInquilino(int id, InquilinoEntity inquilino);

    Task MarkDeleted(int id);
}