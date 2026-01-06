using AdminDepartamentos.API.Models.UnidadHabitacional;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;

namespace AdminDepartamentos.API.Extentions;

public static class UnidadHabitacionalViewExtenticion
{
    public static UnidadHabitacionalViewModel ConvertUnidadHabitacionalViewModelToUnidadHabitacionalModel(
        this UnidadHabitacionalModel unidadHabitacionalModel)
    {
        return new UnidadHabitacionalViewModel
        {
            IdUnidadHabitacional = unidadHabitacionalModel.IdUnidadHabitacional,
            Name = unidadHabitacionalModel.Name,
            Tipo = unidadHabitacionalModel.Tipo,
            LightCode = unidadHabitacionalModel.LightCode,
            Occupied = unidadHabitacionalModel.Occupied,
            InquilinoActual = unidadHabitacionalModel.InquilinoActual,
            Interesados = unidadHabitacionalModel.Interesados
        };
    }

    public static UnidadHabitacionalOccuppiedModel ConvertUnidadHabitacionalEntityToUnidadHabitacionalOccuppiedModel(
        this UnidadHabitacionalEntity unidadHabitacional)
    {
        return new UnidadHabitacionalOccuppiedModel
        {
            IdUnidadHabitacional = unidadHabitacional.IdUnidadHabitacional,
            Name = unidadHabitacional.Name,
            Tipo = unidadHabitacional.Tipo,
            LightCode = unidadHabitacional.LightCode,
            Occupied = unidadHabitacional.Occupied,
            IdInquilinoActual = unidadHabitacional.IdInquilinoActual,
            InquilinoActual = unidadHabitacional.InquilinoActual
                ?.ConvertInquilinoEntityToUnidadHabitacionalGetByInquilinoModel()
        };
    }

    public static UnidadHabitacionalGetByInquilinoModel ConvertInquilinoEntityToUnidadHabitacionalGetByInquilinoModel(
        this InquilinoEntity inquilino)
    {
        return new UnidadHabitacionalGetByInquilinoModel
        {
            IdInquilino = inquilino.IdInquilino,
            FirstName = inquilino.FirstName,
            LastName = inquilino.LastName,
            Cedula = inquilino.Cedula,
            Telefono = inquilino.Telefono
        };
    }
}