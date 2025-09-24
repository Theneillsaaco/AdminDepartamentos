using AdminDepartamentos.API.Models.InteresadoModels;
using AdminDepartamentos.Domain.Entities;

namespace AdminDepartamentos.API.Extentions;

public static class InteresadoViewExtenticion
{
    public static Interesado ConvertInteresadoUpdateModelToInteresadoEntity(this InteresadoUpdateModel interesadoUpdateModel)
    {
        return new Interesado
        {
            FirstName = interesadoUpdateModel.FirstName,
            LastName = interesadoUpdateModel.LastName,
            Telefono = interesadoUpdateModel.Telefono,
            Fecha = interesadoUpdateModel.Fecha,
            TipoUnidadHabitacional = interesadoUpdateModel.TipoUnidadHabitacional
        };
    }
}