using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastructure.Context.Entities;

namespace AdminDepartamentos.Infrastructure.Mapping;

public static class UnidadHabitacionalMapper
{
    public static UnidadHabitacional ToDomain(this UnidadHabitacionalEntity entity)
    {
        var tipo = TipoUnidadModule.fromString(entity.Tipo);
        
        if (tipo.IsError)
            throw new Exception("Tipo de unidad habitacional es invalido");
        
        var estado = entity.Deleted 
            ? EstadoUnidad.Libre
            : entity.IdInquilinoActual.HasValue 
                ? EstadoUnidad.NewOcupada(entity.IdInquilinoActual.Value) 
                : EstadoUnidad.Libre;

        return new UnidadHabitacional(
            entity.Name,
            tipo.ResultValue,
            entity.LightCode,
            estado,
            entity.Deleted
        );
    }

    public static UnidadHabitacionalEntity ToEntity(this UnidadHabitacional domain, int? idUnidadHabitacional = null)
    {
        int? idInquilinoActual = domain.Estado switch
        {
            EstadoUnidad.Ocupada o => o.Item,
            _ => null
        };

        var tipo =
            domain.Tipo.IsApartamento ? "Apartamento" :
            domain.Tipo.IsLocal ? "Local" :
            throw new Exception("Tipo de unidad habitacional es invalido");
        
        return new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = idUnidadHabitacional ?? 0,
            Name = domain.Name,
            Tipo = tipo,
            LightCode = domain.LightCode,
            IdInquilinoActual = idInquilinoActual,
            Deleted = domain.Deleted
        };
    }

    public static void Apply(this UnidadHabitacionalEntity entity, UnidadHabitacional domain)
    {
        entity.Name = domain.Name;
        entity.LightCode = domain.LightCode;
        entity.Deleted = domain.Deleted;

        entity.IdInquilinoActual = domain.Estado switch
        {
            EstadoUnidad.Ocupada o => o.Item,
            _ => null
        };
        
        entity.Tipo = domain.Tipo.IsApartamento ? "Apartamento" :
                        domain.Tipo.IsLocal ? "Local" :
                        throw new Exception("Tipo de unidad habitacional es invalido");

    }
}
