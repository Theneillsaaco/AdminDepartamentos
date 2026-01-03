using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.Models;

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
}