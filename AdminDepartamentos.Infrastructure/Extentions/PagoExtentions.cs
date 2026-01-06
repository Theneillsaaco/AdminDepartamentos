using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.PagoModels;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class PagoExtentions
{
    /// <summary>
    ///     Metodo para convertir de la entida Pago y Inquilino a PagoInquilinoModel.
    /// </summary>
    /// <param name="pago">Pago.</param>
    /// <param name="inquilino">Inquilino.</param>
    /// <returns></returns>
    public static PagoInquilinoModel ToPagoInquilinoModel(this PagoEntity pago, InquilinoEntity inquilino)
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