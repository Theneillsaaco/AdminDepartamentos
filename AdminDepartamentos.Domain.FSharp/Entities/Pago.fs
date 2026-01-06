namespace AdminDepartamentos.Domain.FSharp.Entities

open System
open AdminDepartamentos.Domain.FSharp.Common
open AdminDepartamentos.Domain.FSharp.ValueObjects

type EstadoPago =
    | Pendiente
    | Retrasado of DateTime
    | Eliminado
    
type Pago = {
    NumDeposito : int option
    Monto : decimal
    FechaPago : FechaPago
    Estado : EstadoPago
    Email : bool
}

module Pago =
    let create numDeposito monto fechaPago =
        if monto <= 0m then
            Error (ValidationError "Monto invalido")
        else
            Ok {
                NumDeposito = numDeposito
                Monto = monto
                FechaPago = fechaPago
                Estado = Pendiente
                Email = true
            }
            
    let update numDeposito monto fechapago pago =
        if monto <= 0m then
            Error (ValidationError "Monto invalido")
        else
            Ok {
                pago with
                    NumDeposito = numDeposito
                    Monto = monto
                    FechaPago = fechapago
            }
    
    let checkRetraso (today: DateTime) (pago: Pago)=
        let day = FechaPago.value pago.FechaPago
        
        let isLate =
            if today.Month = 2 && today.Day = 29 && day = 30 then true
            else today.Day > day
            
        match isLate, pago.Estado with
        | true, EstadoPago.Pendiente ->
            { pago with Estado = Retrasado today }
        | false, EstadoPago.Retrasado _ ->
            { pago with Estado = Pendiente }
        | _ -> pago

    let markDeleted pago =
        { pago with Estado = Eliminado }
        
    let markRetrasado date pago =
        { pago with Estado = Retrasado date }