namespace AdminDepartamentos.Domain.FSharp.Services

open AdminDepartamentos.Domain.FSharp.Entities
open AdminDepartamentos.Domain.FSharp.ValueObjects
open AdminDepartamentos.Domain.Models

module InquilinoFactory =
    
    let create (inquilinoDto: InquilinoDto) (pagoDto: PagoDto) =
        let numDeposito =
            if pagoDto.NumDeposito = 0 then None
            else Some pagoDto.NumDeposito
        
        FechaPago.create pagoDto.FechaPagoInDays
        |> Result.bind (fun fecha ->
            Pago.create
                numDeposito
                pagoDto.Monto
                fecha
            |> Result.bind (fun pago ->
                Inquilino.create
                    inquilinoDto.FirstName
                    inquilinoDto.LastName
                    inquilinoDto.Cedula
                    inquilinoDto.NumTelefono
                    pago
            )
        )
        