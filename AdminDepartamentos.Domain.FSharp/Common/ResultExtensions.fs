namespace AdminDepartamentos.Domain.FSharp.Common

module ResultExtensions =
    let bind f r =
        match r with
        | Ok v -> f v
        | Error e -> Error e
