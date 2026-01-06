using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;

namespace AdminDepartamentos.API.Extentions;

public static class InquilinoViewExtenticion
{
    public static InquilinoViewModel ConvertInquilinoViewModelToInquilinoModel(this InquilinoModel inquilinoModel)
    {
        return new InquilinoViewModel
        {
            IdInquilino = inquilinoModel.IdInquilino,
            FirstName = inquilinoModel.FirstName,
            LastName = inquilinoModel.LastName,
            Cedula = inquilinoModel.Cedula,
            NumTelefono = inquilinoModel.NumTelefono
        };
    }

    public static InquilinoViewModel ConvertInquilinoEntityToInquilinoViewModel(this InquilinoEntity inquilino)
    {
        return new InquilinoViewModel
        {
            IdInquilino = inquilino.IdInquilino,
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            Cedula = inquilino.Cedula,
            NumTelefono = inquilino.Telefono
        };
    }

    public static InquilinoEntity ConvertInquilinoUpdateModelToInquilinoEntity(this InquilinoUpdateModel inquilino)
    {
        return new InquilinoEntity
        {
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            Cedula = inquilino.Cedula,
            Telefono = inquilino.NumTelefono
        };
    }
}