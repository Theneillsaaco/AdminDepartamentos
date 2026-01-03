using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Extentions;

public static class InteresadoExtentions
{
    public static InteresadoModel ConvertInteresadoEntityToInteresadoModel(this Interesado interesado)
    {
        return new InteresadoModel
        {
            IdInteresado = interesado.IdInteresado,
            FirstName = interesado.FirstName,
            LastName = interesado.LastName,
            Telefono = interesado.Telefono,
            Fecha = interesado.Fecha,
            TipoUnidadHabitacional = interesado.TipoUnidadHabitacional,
        };
    }

    public static Interesado ConvertInteresadoDtoToInteresadoEntity(this InteresadoDto dto)
    {
        return new Interesado(
            dto.FirstName,
            dto.LastName,
            dto.Telefono,
            dto.TipoUnidadHabitacional
        );
    }
}