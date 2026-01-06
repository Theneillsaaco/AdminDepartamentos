using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Core.Interfaces;
using AdminDepartamentos.Infrastructure.Models.PagoModels;

namespace AdminDepartamentos.Infrastructure.Interfaces;

public interface IPagoRepository : IBaseRepository<PagoEntity>
{
    Task<List<PagoInquilinoModel>> GetPago();

    Task<bool> UpdatePago(int id, PagoEntity pago);
    
    Task<List<PagoEntity>> GetPagosForRetraso();
    
    Task<List<PagoEntity>> GetRetrasosWithoutEmail();
}