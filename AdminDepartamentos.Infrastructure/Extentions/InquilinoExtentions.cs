using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartament.Infrastructure.Extentions;

public static class InquilinoExtentions
{
    public static InquilinoModel ConvertInquilinoEntityToInquilinoModel(this Inquilino inquilino)
    {
        InquilinoModel inquilinoModel = new InquilinoModel()
        {
            IdInquilino = inquilino.IdInquilino,
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            NumDepartamento = inquilino.NumDepartamento,
        };

        return inquilinoModel;
    }
}