using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Extentions;

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
}