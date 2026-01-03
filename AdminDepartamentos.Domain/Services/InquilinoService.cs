using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Extentions;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Services;

public class InquilinoService
{
    public Inquilino Save(InquilinoDto inquilinoDto, PagoDto pagoDto)
    {
        if (inquilinoDto is null)
            throw new ArgumentNullException(nameof(inquilinoDto), "El inquilino no puede ser null.");

        if (pagoDto is null)
            throw new ArgumentNullException(nameof(pagoDto), "El pago no puede ser null.");

        var inquilino = Inquilino.Create(
            inquilinoDto.FirstName,
            inquilinoDto.LastName,
            inquilinoDto.Cedula,
            inquilinoDto.NumTelefono
        );

        var pago = Pago.Create(
            pagoDto.NumDeposito,
            pagoDto.Monto,
            pagoDto.FechaPagoInDays
        );

        inquilino.AsignPago(pago);

        return inquilino;
    }

    #region Fields

    private readonly IInquilinoRepository _inquilinoRepository;
    private readonly IPagoRepository _pagoRepository;
    
    public InquilinoService(IInquilinoRepository inquilinoRepository, IPagoRepository pagoRepository)
    {
        _inquilinoRepository = inquilinoRepository;
        _pagoRepository = pagoRepository;
    }

    #endregion
}