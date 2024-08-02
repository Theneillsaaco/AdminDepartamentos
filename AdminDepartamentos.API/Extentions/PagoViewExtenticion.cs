using AdminDepartamentos.API.Models.PagoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

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
            
            //Inquilino
            IdInquilino = pagoInquilinoModel.IdInquilino,
            InquilinoFirstName = pagoInquilinoModel.InquilinoFirstName,
            InquilinoLastName = pagoInquilinoModel.InquilinoLastName
        };
    }
    
    public static PagoGetModel ConvertToPagoGetModel(this Pago pago)
    {
        return new PagoGetModel
        {
            IdPago = pago.IdPago,
            IdInquilino = pago.IdInquilino,
            NumDeposito = pago.NumDeposito,
            FechaPagoInDays = pago.FechaPagoInDays,
            Retrasado = pago.Retrasado,
            Deleted = pago.Deleted
        };
    }

    public static Pago ConverToPagoEntityToPagoUpdateModel(this PagoUpdateModel pagoUpdateModel)
    {
        return new Pago
        {
            NumDeposito = pagoUpdateModel.NumDeposito,
            Monto = pagoUpdateModel.Monto,
            FechaPagoInDays = pagoUpdateModel.FechaPagoInDays
        };
    }
    
    public static Pago ConvertToPagoEntity(this PagoInquilinoModel pagoInquilinoModel)
    {
        return new Pago
        {
            IdPago = pagoInquilinoModel.IdPago,
            IdInquilino = pagoInquilinoModel.IdInquilino,
            NumDeposito = pagoInquilinoModel.NumDeposito,
            FechaPagoInDays = pagoInquilinoModel.FechaPagoInDays,
            Retrasado = pagoInquilinoModel.Retrasado
        };
    }
}