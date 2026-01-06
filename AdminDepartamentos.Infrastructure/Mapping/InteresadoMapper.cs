using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context.Entities;

namespace AdminDepartamentos.Infrastructure.Mapping;

public static class InteresadoMapper
{
   public static interesado ToDomain(this InteresadoEntity entity)
    {
        var tipo = TipoUnidadModule.fromString(entity.TipoUnidadHabitacional);

        if (tipo.IsError)
            throw new Exception("Tipo de unidad habitacional es invalido");
        
        return new interesado(
            entity.FirstName,
            entity.LastName,
            entity.Telefono,
            tipo.ResultValue,
            entity.Fecha,
            entity.Deleted
        );
    }

    public static InteresadoEntity ToEntity(this interesado domain, int? idInteresado = null)
    {
        var tipo =
            domain.TipoUnidadHabitacional.IsApartamento ? "Apartamento" :
            domain.TipoUnidadHabitacional.IsLocal ? "Local" :
            throw new Exception("Tipo de unidad habitacional es invalido");

        return new InteresadoEntity
        {
            IdInteresado = idInteresado ?? 0,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            Telefono = domain.Telefono,
            TipoUnidadHabitacional = tipo,
            Fecha = domain.Fecha,
            Deleted = domain.Deleted
        };
    }
    
    public static void Apply(this InteresadoEntity entity, interesado domain)
    {
        entity.FirstName = domain.FirstName;
        entity.LastName = domain.LastName;
        entity.Telefono = domain.Telefono;
        entity.Fecha = domain.Fecha;
        entity.Deleted = domain.Deleted;
        
        entity.TipoUnidadHabitacional = domain.TipoUnidadHabitacional.IsApartamento ? "Apartamento" :
                                        domain.TipoUnidadHabitacional.IsLocal ? "Local" :
                                        throw new Exception("Tipo de unidad habitacional es invalido");

    }
}
