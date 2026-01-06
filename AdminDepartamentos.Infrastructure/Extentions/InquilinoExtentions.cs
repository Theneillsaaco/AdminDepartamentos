using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class InquilinoExtentions
{
    public static InquilinoModel ConvertInquilinoEntityToInquilinoModel(this InquilinoEntity inquilino)
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