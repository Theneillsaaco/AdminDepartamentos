using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Domain.FSharp.ValueObjects;
using AdminDepartamentos.Infrastucture.Context.Entities;
using Pago = AdminDepartamentos.Domain.FSharp.Entities.Pago;

namespace AdminDepartamentos.Infrastucture.Mapping;

public static class PagoMapper
{
    public static Pago ToDomain(this PagoEntity entity)
    {
        var fechaResult = FechaPagoModule.create(entity.FechaPagoInDays);
        if (fechaResult.IsError)
            throw new Exception("Fecha del pago es invalida");

        var pagoResult = PagoModule.create(
            entity.NumDeposito,
            entity.Monto,
            fechaResult.ResultValue
        );
        
        if (pagoResult.IsError)
            throw new Exception("Pago es invalido");
        
        var pago = pagoResult.ResultValue;
        
        if (entity.Deleted)
            return PagoModule.markDeleted(pago);
        
        if (entity.Retrasado && entity.RetrasadoDate.HasValue)
            return PagoModule.markRetrasado(entity.RetrasadoDate.Value, pago);

        return pago;
    }

    public static PagoEntity ToEntity(this Pago domain, int inquilinoId)
    {
        var entity = new PagoEntity
        {
            IdInquilino = inquilinoId,
            NumDeposito = domain.NumDeposito?.Value,
            Monto = domain.Monto,
            FechaPagoInDays = FechaPagoModule.value(domain.FechaPago),
            Email = domain.Email,
            Deleted = false,
            Retrasado = false,
            RetrasadoDate = null
        };

        
        switch (domain.Estado.Tag)
        {
            case 0: // Pendiente
                break;

            case 1: // Retrasado of DateTime
                var retrasado =
                    (EstadoPago.Retrasado)domain.Estado;

                entity.RetrasadoDate = retrasado.Item;
                break;

            case 2: // Eliminado
                entity.Deleted = true;
                break;
        }
        
        return entity;
    }

    public static void Apply(this PagoEntity entity, Pago domain)
    {
        entity.NumDeposito = domain.NumDeposito?.Value;
        entity.Monto = domain.Monto;
        entity.FechaPagoInDays = FechaPagoModule.value(domain.FechaPago);
        entity.Email = domain.Email;
        
        switch (domain.Estado.Tag)
        {
            case 0: // Pendiente
                entity.Deleted = false;
                entity.Retrasado = false;
                entity.RetrasadoDate = null;
                break;
            
            case 1: // Retrasado of DateTime
                var retrasado = (EstadoPago.Retrasado)domain.Estado;
                entity.Deleted = false;
                entity.Retrasado = true;
                entity.RetrasadoDate = retrasado.Item;
                break;
            
            case 2: // Eliminado
                entity.Deleted = true;
                entity.Retrasado = false;
                entity.RetrasadoDate = null;
                break;
        }
    }
}