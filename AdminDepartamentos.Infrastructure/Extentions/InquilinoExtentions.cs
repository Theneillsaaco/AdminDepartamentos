using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartament.Infrastructure.Extentions;

public static class InquilinoExtentions
{
    public static InquilinoModel ConvertInquilinoEntityToInquilinoModel(this Inquilino inquilino)
    {
        return new InquilinoModel 
        {
            IdInquilino = inquilino.IdInquilino,
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            NumDepartamento = inquilino.NumDepartamento,
        };
    }

    public static Inquilino ConvertEntityInquilinoToInquilinoDto(this InquilinoDto inquilinoDto)
    {
        return new Inquilino
        {
            FirstName = inquilinoDto.FirstName,
            LastName = inquilinoDto.LastName,
            Cedula = inquilinoDto.Cedula,
            NumDepartamento = inquilinoDto.NumDepartamento,
            NumTelefono = inquilinoDto.NumTelefono,
        };
    }
    
    public static Pago ConvertEntityInquilinoToPagoDto(this PagoDto pagoDto, int IdInquilino)
    {
        return new Pago
        {
            Monto = pagoDto.Monto,
            NumDeposito =  pagoDto.NumDeposito
        };
    }
}

