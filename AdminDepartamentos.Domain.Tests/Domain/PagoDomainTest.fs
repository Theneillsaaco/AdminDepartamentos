namespace AdminDepartamentos.Domain.Test

open System
open Xunit
open AdminDepartamentos.Domain.FSharp.Entities
open AdminDepartamentos.Domain.FSharp.ValueObjects

module PagoDomainTest =
    
    let unwrapResult = function
        | Ok v -> v
        | Error e -> failwith $"Unexpected error: {e}"

    let createFecha day = 
        FechaPago.create day |> unwrapResult

    [<Fact>]
    let ``Create returns Error when monto <= 0`` () =
        // Arrange
        let fecha = createFecha 10

        let result = Pago.create None 0m fecha

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``Create creates pago with Pendiente state`` () =
        let fecha = createFecha 15

        let result = Pago.create (Some 123) 1200m fecha

        match result with
        | Ok pago ->
            Assert.Equal(EstadoPago.Pendiente, pago.Estado)
            Assert.True(pago.Email)
            Assert.Equal(1200m, pago.Monto)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``update returns Error when monto <= 0`` () =
        let fecha = createFecha 10

        let pago =
            {
                NumDeposito = None
                Monto = 500m
                FechaPago = fecha
                Estado = Pendiente
                Email = true
            }

        let result = Pago.update None 0m fecha pago

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``update updates monto and fecha`` () =
        let fechaOriginal = createFecha 10
        let fechaNueva = createFecha 20

        let pago =
            {
                NumDeposito = None
                Monto = 500m
                FechaPago = fechaOriginal
                Estado = Pendiente
                Email = true
            }

        let result = Pago.update (Some 999) 1500m fechaNueva pago

        match result with
        | Ok updated ->
            Assert.Equal(1500m, updated.Monto)
            Assert.Equal(fechaNueva, updated.FechaPago)
            Assert.Equal(Some 999, updated.NumDeposito)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``checkRetraso marks pago as Retrasado when late`` () =
        let fecha = createFecha 10

        let pago =
            {
                NumDeposito = None
                Monto = 800m
                FechaPago = fecha
                Estado = Pendiente
                Email = true
            }

        let today = DateTime(2025, 3, 20)

        let updated = Pago.checkRetraso today pago

        match updated.Estado with
        | EstadoPago.Retrasado date ->
            Assert.Equal(today, date)
        | _ ->
            Assert.True(false)

    [<Fact>]
    let ``checkRetraso returns Pendiente when not late`` () =
        let fecha = createFecha 25

        let pago =
            {
                NumDeposito = None
                Monto = 800m
                FechaPago = fecha
                Estado = EstadoPago.Retrasado(DateTime(2025, 1, 1))
                Email = true
            }

        let today = DateTime(2025, 1, 10)

        let updated = Pago.checkRetraso today pago

        Assert.Equal(EstadoPago.Pendiente, updated.Estado)

    [<Fact>]
    let ``markDeleted sets estado Eliminado`` () =
        let fecha = createFecha 10

        let pago =
            {
                NumDeposito = None
                Monto = 500m
                FechaPago = fecha
                Estado = Pendiente
                Email = true
            }

        let deleted = Pago.markDeleted pago

        Assert.Equal(EstadoPago.Eliminado, deleted.Estado)

    [<Fact>]
    let ``markRetrasado sets estado Retrasado`` () =
        let fecha = createFecha 10

        let pago =
            {
                NumDeposito = None
                Monto = 500m
                FechaPago = fecha
                Estado = Pendiente
                Email = true
            }

        let date = DateTime(2025, 3, 1)

        let updated = Pago.markRetrasado date pago

        match updated.Estado with
        | EstadoPago.Retrasado d ->
            Assert.Equal(date, d)
        | _ ->
            Assert.True(false)
