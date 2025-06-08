using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class InquilinoExtentions
{
    public static InquilinoModel ConvertInquilinoEntityToInquilinoModel(this Inquilino inquilino)
    {
        return new InquilinoModel
        {
            IdInquilino = inquilino.IdInquilino,
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            Cedula = inquilino.Cedula,
            NumTelefono = inquilino.Telefono
        };
    }

    public static Inquilino ConvertInquilinoDtoToInquilinoEntity(this InquilinoDto inquilinoDto)
    {
        return new Inquilino
        {
            FirstName = inquilinoDto.FirstName,
            LastName = inquilinoDto.LastName,
            Cedula = inquilinoDto.Cedula,
            Telefono = inquilinoDto.NumTelefono
        };
    }
}