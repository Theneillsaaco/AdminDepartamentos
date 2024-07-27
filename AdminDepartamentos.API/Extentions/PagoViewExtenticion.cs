using AdminDepartamentos.API.Models.PagoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Extentions;

public static class PagoViewExtenticion
{
    public static PagoViewModel ConvertToPagoModel(this PagoInquilinoModel pagoInquilinoModel)
    {
        return new PagoViewModel
        {
            IdPago = pagoInquilinoModel.IdPago,
            IdInquilino = pagoInquilinoModel.IdInquilino,
            NumDeposito = pagoInquilinoModel.NumDeposito,
            FechaPago = pagoInquilinoModel.FechaPago,
            Retrasado = pagoInquilinoModel.Retrasado
        };
    }
    
    public static PagoGetByInquilinoModel ConvertToPagoGetByInquilinoModel(this PagoInquilinoModel pagoInquilinoModel)
    {
        return new PagoGetByInquilinoModel
        {
            //Pago
            IdPago = pagoInquilinoModel.IdPago,
            NumDeposito = pagoInquilinoModel.NumDeposito,
            FechaPago = pagoInquilinoModel.FechaPago,
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
            FechaPago = pago.FechaPago,
            Retrasado = pago.Retrasado,
            Deleted = pago.Deleted
        };
    }

    public static Pago ConverToPagoEntityToPagoUpdateModel(this PagoUpdateModel pagoUpdateModel)
    {
        return new Pago
        {
            IdInquilino = pagoUpdateModel.IdInquilino,
            NumDeposito = pagoUpdateModel.NumDeposito,
            Monto = pagoUpdateModel.Monto,
            FechaPago = pagoUpdateModel.FechaPago
        };
    }
}