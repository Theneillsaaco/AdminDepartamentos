using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartament.Infrastructure.Extentions;

public static class PagoExtentions
{
    /// <summary>
    ///     Metodo para convertir de la entida Pago y Inquilino a PagoInquilinoModel.
    /// </summary>
    /// <param name="pago">Pago.</param>
    /// <param name="inquilino">Inquilino.</param>
    /// <returns></returns>
    public static PagoInquilinoModel ConvertPagoEntityToPagoInquilinoModel(this Pago pago, Inquilino inquilino)
    {
        return new PagoInquilinoModel
        {
            IdPago = pago.IdPago,
            FechaPagoInDays = pago.FechaPagoInDays,
            NumDeposito = pago.NumDeposito,
            Monto = pago.Monto,
            Retrasado = pago.Retrasado,

            // Inquilino
            IdInquilino = inquilino.IdInquilino,
            InquilinoFirstName = inquilino.FirstName,
            InquilinoLastName = inquilino.LastName
        };
    }
}