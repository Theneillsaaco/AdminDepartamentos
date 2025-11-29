using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class UnidadHabitacionalExtentions
{
    public static UnidadHabitacionalModel ConvertUnidadHabitacionalEntityToUnidadHabitacionalModel(this UnidadHabitacional unidadHabitacional)
    {
        return new UnidadHabitacionalModel
        {
            IdUnidadHabitacional = unidadHabitacional.IdUnidadHabitacional,
            Name = unidadHabitacional.Name,
            Tipo = unidadHabitacional.Tipo,
            LightCode = unidadHabitacional.LightCode,
            occcupied = unidadHabitacional.Occupied,
            InquilinoActual = unidadHabitacional.InquilinoActual?.ConvertInquilinoEntityToInquilinoModel(),
            Interesados = unidadHabitacional.Interesados.Select(uni => uni.ConvertInteresadoEntityToInteresadoModel()).ToList()
        };
    }

    public static UnidadHabitacional ConvertUnidadHabitacionalDtoToUnidadHabitacionalEntity(
        this UnidadHabitacionalDto unidadHabitacionalDto)
    {
        return new UnidadHabitacional
        {
            Name = unidadHabitacionalDto.Name,
            Tipo = unidadHabitacionalDto.Tipo,
            LightCode = unidadHabitacionalDto.LightCode
        };
    }
    
}