using AdminDepartamentos.API.Models.UnidadHabitacional;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Extentions;

public static class UnidadHabitacionalViewExtenticionModel
{
    public static UnidadHabitacionalViewModel ConvertUnidadHabitacionalViewModelToUnidadHabitacionalModel(
        this UnidadHabitacionalModel unidadHabitacionalModel)
    {
        return new UnidadHabitacionalViewModel
        {
            Name = unidadHabitacionalModel.Name,
            Tipo = unidadHabitacionalModel.Tipo,
            occcupied = unidadHabitacionalModel.occcupied,
            InquilinoActual = unidadHabitacionalModel.InquilinoActual,
            Interesados = unidadHabitacionalModel.Interesados
        };
    }
    
    public static UnidadHabitacional ConvertUnidadHabitacionalEntityToUnidadHabitacionalUpdateModel(
        this UnidadHabitacionalUpdateModel unidadHabitacionalUpdateModel)
    {
        return new UnidadHabitacional
        {
            Name = unidadHabitacionalUpdateModel.Name,
            Tipo = unidadHabitacionalUpdateModel.Tipo
        };
    }

    public static UnidadHabitacionalOccuppiedModel ConvertUnidadHabitacionalEntityToUnidadHabitacionalOccuppiedModel(
        this UnidadHabitacional unidadHabitacional)
    {
        return new UnidadHabitacionalOccuppiedModel
        {
            IdUnidadHabitacional = unidadHabitacional.IdUnidadHabitacional,
            Name = unidadHabitacional.Name,
            Tipo = unidadHabitacional.Tipo,
            Occcupied = unidadHabitacional.Occcupied,
            IdInquilinoActual = unidadHabitacional.IdInquilinoActual,
            InquilinoActual = unidadHabitacional.InquilinoActual?.ConvertInquilinoEntityToUnidadHabitacionalGetByInquilinoModel()
        };
    }

    public static UnidadHabitacionalGetByInquilinoModel ConvertInquilinoEntityToUnidadHabitacionalGetByInquilinoModel(
        this Inquilino inquilino)
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