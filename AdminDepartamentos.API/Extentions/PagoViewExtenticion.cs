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

    public static Pago ConverToPagoEntityToPagoUpdateModel(this PagoUpdateModel pagoUpdateModel)
    {
        return new Pago
        {
            // Inquilino
            IdInquilino = pagoUpdateModel.IdInquilino,
            
            // Pago
            NumDeposito = pagoUpdateModel.NumDeposito,
            Monto = pagoUpdateModel.Monto,
            FechaPago = pagoUpdateModel.FechaPago
        };
    }
}