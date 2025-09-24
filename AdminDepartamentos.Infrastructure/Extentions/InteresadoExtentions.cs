using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Infrastructure.Extentions;

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

    public static Interesado ConvertInteresadoDtoToInteresadoEntity(this InteresadoDto interesadoDto)
    {
        return new Interesado()
        {
            FirstName = interesadoDto.FirstName,
            LastName = interesadoDto.LastName,
            Telefono = interesadoDto.Telefono,
            TipoUnidadHabitacional = interesadoDto.TipoUnidadHabitacional
        };
    }
}