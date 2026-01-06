namespace AdminDepartamentos.Domain.FSharp.Entities

open System
open AdminDepartamentos.Domain.FSharp.Common
open AdminDepartamentos.Domain.FSharp.ValueObjects

type interesado = {
    FirstName: string
    LastName: string
    Telefono: string
    TipoUnidadHabitacional: TipoUnidad
    Fecha: DateTime
    Deleted: bool
}

module Interesado =
    let create firstName lastName telefono tipoUnidad =
        if String.IsNullOrWhiteSpace firstName then
            Error (ValidationError "Nombre Obligatorio")
        elif String.IsNullOrWhiteSpace lastName then
            Error (ValidationError "Apellido Obligatorio")
        elif String.IsNullOrWhiteSpace telefono then
            Error (ValidationError "Telefono Obligatorio")
        else
            Ok {
                FirstName = firstName
                LastName = lastName
                Telefono = telefono
                TipoUnidadHabitacional = tipoUnidad
                Fecha = DateTime.Now
                Deleted = false
            }

    let update firstName lastName telefono tipoUnidad interesado =
        if interesado.Deleted then
            Error (ValidationError "Interesado Eliminado")
        elif String.IsNullOrWhiteSpace firstName then
            Error (ValidationError "Nombre Obligatorio")
        elif String.IsNullOrWhiteSpace lastName then
            Error (ValidationError "Apellido Obligatorio")
        elif String.IsNullOrWhiteSpace telefono then
            Error (ValidationError "Telefono Obligatorio")
        else
            Ok {
                interesado with
                    FirstName = firstName
                    LastName = lastName
                    Telefono = telefono
                    TipoUnidadHabitacional = tipoUnidad
            }

    let markDeleted interesado =
        if interesado.Deleted then
            interesado
        else { interesado with Deleted = true }