using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Extentions;

public static class InquilinoViewExtenticion
{
    public static InquilinoViewModel ConvertToInquilinoModel(this InquilinoModel inquilinoModel)
    {
        return new InquilinoViewModel
        {
            IdInquilino = inquilinoModel.IdInquilino,
            FirstName = inquilinoModel.FirstName,
            LastName = inquilinoModel.LastName,
            NumDepartamento = inquilinoModel.NumDepartamento
        };
    }

    public static InquilinoPagoModel ConvertToInquilinoPagoModel(this InquilinoSaveModel inquilinoSaveModel)
    {
        return new InquilinoPagoModel
        {
            Inquilino = new Inquilino
            {
                FirstName = inquilinoSaveModel.FirstName,
                LastName = inquilinoSaveModel.LastName,
                Cedula = inquilinoSaveModel.Cedula,
                NumDepartamento = inquilinoSaveModel.NumDepartamento,
                NumTelefono = inquilinoSaveModel.NumTelefono,
                CreationDate = DateTime.Now
            },
            
            Pago = new Pago
            {
                Monto = inquilinoSaveModel.Monto,
                NumDeposito = inquilinoSaveModel.NumDeposito
            }
        };
    }
    
    public static Inquilino ConvertToEntityInquilino(this InquilinoUpdateModel inquilinoUpdateModel)
    {
        return new Inquilino
        {
            FirstName = inquilinoUpdateModel.FirstName,
            LastName = inquilinoUpdateModel.LastName,
            NumDepartamento = inquilinoUpdateModel.NumDepartamento,
            Cedula = inquilinoUpdateModel.Cedula,
            NumTelefono = inquilinoUpdateModel.NumTelefono,
            ModifyDate = inquilinoUpdateModel.ModifyDate
        };
    }
    
    public static Inquilino ConvertToEntityInquilino(this InquilinoDetailModel inquilinoDetailModel)
    {
        return new Inquilino
        {
            IdInquilino = inquilinoDetailModel.IdInquilino,
            FirstName = inquilinoDetailModel.FirstName,
            LastName = inquilinoDetailModel.LastName,
            NumDepartamento = inquilinoDetailModel.NumDepartamento,
            Cedula = inquilinoDetailModel.Cedula,
            NumTelefono = inquilinoDetailModel.NumTelefono,
            CreationDate = inquilinoDetailModel.CreationDate
        };
    }
}