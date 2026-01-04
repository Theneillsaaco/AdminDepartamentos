namespace AdminDepartamentos.Domain.FSharp.Common

type DomainError =
    | ValidationError of string
    | BusinessRuleError of string