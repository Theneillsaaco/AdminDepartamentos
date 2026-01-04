using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastucture.Context.Entities;
using AdminDepartamentos.Infrastucture.Core.Interfaces;

namespace AdminDepartamentos.Infrastucture.Interfaces;

public interface IPagoRepository : IBaseRepository<PagoEntity>
{
    Task<List<PagoInquilinoModel>> GetPago();

    Task<bool> UpdatePago(int id, PagoEntity pago);
    
    Task<List<PagoEntity>> GetPagosForRetraso();
    
    Task<List<PagoEntity>> GetRetrasosWithoutEmail();
}