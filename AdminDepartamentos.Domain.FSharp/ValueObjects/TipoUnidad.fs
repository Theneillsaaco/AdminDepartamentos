namespace AdminDepartamentos.Domain.FSharp.ValueObjects

open AdminDepartamentos.Domain.FSharp.Common

type TipoUnidad =
    | Apartamento
    | Local
    
module TipoUnidad =
    let fromString (value: string) =
        match value.Trim().ToLowerInvariant() with
        | "apartamento" -> Ok Apartamento
        | "local" -> Ok Local
        | _ -> Error (ValidationError "Tipo de unidad invalido (solo Apartamento o Local)")