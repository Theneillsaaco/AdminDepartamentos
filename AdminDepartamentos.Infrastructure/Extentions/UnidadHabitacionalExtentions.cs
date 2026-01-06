using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class UnidadHabitacionalExtentions
{
    public static UnidadHabitacionalModel ConvertUnidadHabitacionalEntityToUnidadHabitacionalModel(
        this UnidadHabitacionalEntity unidadHabitacional)
    {
        return new UnidadHabitacionalModel
        {
            IdUnidadHabitacional = unidadHabitacional.IdUnidadHabitacional,
            Name = unidadHabitacional.Name,
            Tipo = unidadHabitacional.Tipo,
            LightCode = unidadHabitacional.LightCode,
            Occupied = unidadHabitacional.Occupied,
            InquilinoActual = unidadHabitacional.InquilinoActual?.ConvertInquilinoEntityToInquilinoModel(),
            Interesados = unidadHabitacional.Interesados.Select(uni => uni.ConvertInteresadoEntityToInteresadoModel())
                .ToList()
        };
    }

    public static UnidadHabitacionalEntity ConvertUnidadHabitacionalDtoToUnidadHabitacionalEntity(
        this UnidadHabitacionalDto unidadHabitacionalDto)
    {
        return new UnidadHabitacionalEntity
        {
            Name = unidadHabitacionalDto.Name,
            Tipo = unidadHabitacionalDto.Tipo,
            LightCode = unidadHabitacionalDto.LightCode
        };
    }
}