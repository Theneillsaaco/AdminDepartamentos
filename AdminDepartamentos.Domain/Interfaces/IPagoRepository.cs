using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IPagoRepository : IBaseRepository<Pago>
{
    Task<List<PagoInquilinoModel>> GetPago();
}