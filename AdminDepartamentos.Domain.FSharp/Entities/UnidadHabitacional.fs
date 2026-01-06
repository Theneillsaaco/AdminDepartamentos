namespace AdminDepartamentos.Domain.FSharp.Entities

open System
open AdminDepartamentos.Domain.FSharp.Common
open AdminDepartamentos.Domain.FSharp.ValueObjects

type EstadoUnidad = 
    | Libre
    | Ocupada of int

type UnidadHabitacional = {
    Name: string
    Tipo: TipoUnidad
    LightCode: string
    Estado: EstadoUnidad
    Deleted: bool
}

module UnidadHabitacional =
    let create name tipo lightCode =
        if String.IsNullOrWhiteSpace name then
            Error (ValidationError "Nombre Obligatorio")
        elif String.IsNullOrWhiteSpace lightCode then
            Error (ValidationError "LightCode Obligatorio")
        else
            match tipo with
            | Apartamento
            | Local ->
                Ok {
                    Name = name
                    Tipo = tipo
                    LightCode = lightCode
                    Estado = Libre
                    Deleted = false
                }

    let updateInfo name tipo lightCode unidad =
        if unidad.Deleted then
            Error (ValidationError "Unidad Habitacional Eliminada")
        elif String.IsNullOrWhiteSpace name then
            Error (ValidationError "Nombre Obligatorio")
        elif String.IsNullOrWhiteSpace lightCode then
            Error (ValidationError "LightCode Obligatorio")
        else
            Ok {
                unidad with
                    Name = name
                    Tipo = tipo
                    LightCode = lightCode
            }

    let assingInquilino inquilinoId unidad =
        if unidad.Deleted then
            Error (ValidationError "Unidad Habitacional Eliminada")
        else
            match unidad.Estado with
            | Ocupada _ -> 
                Error (ValidationError "Unidad Habitacional Ocupada")
            | Libre -> 
                Ok { unidad with Estado = Ocupada inquilinoId }

    let release unidad = 
        match unidad.Estado with
        | Libre -> unidad
        | Ocupada _ -> { unidad with Estado = Libre }

    let markDeleted unidad =
        if unidad.Deleted then
            unidad
        else
            unidad
            |> release
            |> fun u -> { u with Deleted = true }