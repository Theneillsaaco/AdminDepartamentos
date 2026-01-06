namespace AdminDepartamentos.Domain.FSharp.Entities

open System
open AdminDepartamentos.Domain.FSharp.Common

type Inquilino = {
    FirstName: string
    LastName: string
    Cedula: string
    Telefono : string
    Pago : Pago
    Deleted : bool
} 
    
module Inquilino =
    let create first last cedula telefono pago =
        if String.IsNullOrWhiteSpace cedula then
            Error (ValidationError "Cedula Obligatoria")
        else
            Ok {
                FirstName = first
                LastName = last
                Cedula = cedula
                Telefono = telefono
                Pago = pago
                Deleted = false
            }
    let markDeleted inquilino =
        {
            inquilino with
                Deleted = true
                Pago = Pago.markDeleted inquilino.Pago }
        
    let update first last cedula telefono inquilino =
        if String.IsNullOrWhiteSpace cedula then
            Error (ValidationError "Cedula Obligatoria")
        else
            Ok {
                inquilino with
                    FirstName = first
                    LastName = last
                    Cedula = cedula
                    Telefono = telefono
                    Deleted = false
            }