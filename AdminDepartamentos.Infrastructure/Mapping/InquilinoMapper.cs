using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Infrastructure.Context.Entities;

namespace AdminDepartamentos.Infrastructure.Mapping;

public static class InquilinoMapper
{
    public static Inquilino ToDomain(this InquilinoEntity entity)
    {
        var pago = entity.Pago.ToDomain();

        var result = InquilinoModule.create(
            entity.FirstName,
            entity.LastName,
            entity.Cedula,
            entity.Telefono,
            pago
        );
        
        if (result.IsError)
            throw new Exception("Inquilino es invalido");
        
        var inquilino = result.ResultValue;
        
        if (entity.Deleted)
            return InquilinoModule.markDeleted(inquilino);
        
        return inquilino;
    }

    public static InquilinoEntity ToEntity(this Inquilino domain)
    {
        return new InquilinoEntity
        {
            FirstName = domain.FirstName,
            LastName = domain.LastName,
            Cedula = domain.Cedula,
            Telefono = domain.Telefono,
            Deleted = domain.Deleted,
            CreationDate = DateTime.Now,
        };
    }

    public static void Apply(this InquilinoEntity entity, Inquilino domain)
    {
        entity.FirstName = domain.FirstName;
        entity.LastName = domain.LastName;
        entity.Cedula = domain.Cedula;
        entity.Telefono = domain.Telefono;
        entity.Deleted = domain.Deleted;
        entity.ModifyDate = DateTime.Now;
    }
}