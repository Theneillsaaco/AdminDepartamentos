namespace AdminDepartamentos.Domain.FSharp.ValueObjects

open AdminDepartamentos.Domain.FSharp.Common

type FechaPago = private FechaPago of int

module FechaPago =
    let create day =
        if day < 1 || day > 30 then
            Error (ValidationError "La fecha del pago debe estar entre 1 y 30")
        else
            Ok (FechaPago day)
            
    let value (FechaPago day) = day
