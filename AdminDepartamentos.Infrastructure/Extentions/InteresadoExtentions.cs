using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Models.InteresadoModels;

namespace AdminDepartamentos.Infrastructure.Extentions;

public static class InteresadoExtentions
{
    public static InteresadoModel ConvertInteresadoEntityToInteresadoModel(this InteresadoEntity interesado)
    {
        return new InteresadoModel
        {
            IdInteresado = interesado.IdInteresado,
            FirstName = interesado.FirstName,
            LastName = interesado.LastName,
            Telefono = interesado.Telefono,
            Fecha = interesado.Fecha,
            TipoUnidadHabitacional = interesado.TipoUnidadHabitacional
        };
    }

    public static InteresadoEntity ConvertInteresadoDtoToInteresadoEntity(this InteresadoDto dto)
    {
        return new InteresadoEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Telefono = dto.Telefono,
            TipoUnidadHabitacional = dto.TipoUnidadHabitacional
        };
    }
}