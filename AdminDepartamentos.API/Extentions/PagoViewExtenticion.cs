using AdminDepartamentos.API.Models.PagoModels;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.PagoModels;

namespace AdminDepartamentos.API.Extentions;

public static class PagoViewExtenticion
{
    public static PagoGetByInquilinoModel ConvertToPagoGetByInquilinoModel(this PagoInquilinoModel pagoInquilinoModel)
    {
        return new PagoGetByInquilinoModel
        {
            //Pago
            IdPago = pagoInquilinoModel.IdPago,
            NumDeposito = pagoInquilinoModel.NumDeposito,
            FechaPagoInDays = pagoInquilinoModel.FechaPagoInDays,
            Retrasado = pagoInquilinoModel.Retrasado,
            Monto = pagoInquilinoModel.Monto,

            //Inquilino
            IdInquilino = pagoInquilinoModel.IdInquilino,
            InquilinoFirstName = pagoInquilinoModel.InquilinoFirstName,
            InquilinoLastName = pagoInquilinoModel.InquilinoLastName
        };
    }

    public static PagoGetModel ConvertToPagoGetModel(this PagoEntity pago)
    {
        return new PagoGetModel
        {
            IdPago = pago.IdPago,
            IdInquilino = pago.IdInquilino,
            NumDeposito = pago.NumDeposito,
            FechaPagoInDays = pago.FechaPagoInDays,
            Retrasado = pago.Retrasado,
            Monto = pago.Monto,
            Email = pago.Email,
            Deleted = pago.Deleted
        };
    }

    public static PagoEntity ConverToPagoEntityToPagoUpdateModel(this PagoUpdateModel pagoUpdateModel)
    {
        return new PagoEntity
        {
            NumDeposito = pagoUpdateModel.NumDeposito,
            Monto = pagoUpdateModel.Monto,
            FechaPagoInDays = pagoUpdateModel.FechaPagoInDays
        };
    }

    public static PagoEntity ConvertToPagoEntity(this PagoInquilinoModel pagoInquilinoModel)
    {
        return new PagoEntity
        {
            IdPago = pagoInquilinoModel.IdPago,
            IdInquilino = pagoInquilinoModel.IdInquilino,
            NumDeposito = pagoInquilinoModel.NumDeposito,
            FechaPagoInDays = pagoInquilinoModel.FechaPagoInDays,
            Retrasado = pagoInquilinoModel.Retrasado
        };
    }

    public static PagoWithoutEmail ConvertPagoEntityToPagoWithoutEmail(this PagoEntity pago)
    {
        return new PagoWithoutEmail
        {
            IdPago = pago.IdPago,
            Retrasado = pago.Retrasado,
            Deleted = pago.Deleted,
            Email = pago.Email,

            // Inquilino
            IdInquilino = pago.IdInquilino,
            InquilinoFirstName = pago.Inquilino.FirstName,
            InquilinoLastName = pago.Inquilino.LastName
        };
    }
}