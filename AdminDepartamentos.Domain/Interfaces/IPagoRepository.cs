﻿using AdminDepartamentos.Domain.Core.Interfaces;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IPagoRepository : IBaseRepository<Pago>
{
    Task<List<PagoInquilinoModel>> GetPago();

    /// <summary>
    /// Desatachar la entidad existente para evitar conflictos de seguimiento.
    /// </summary>
    void DetachEntity(Pago entity);

    void CheckRetraso(Pago pago);

    Task<List<Pago>> GetRetrasosWithoutEmail();
}