namespace AdminDepartamentos.Unit.Test.Domain

open Xunit
open AdminDepartamentos.Domain.FSharp.Entities
open AdminDepartamentos.Domain.FSharp.ValueObjects

module InquilinoDomainTest =

    let unwrapResult = function
        | Ok v -> v
        | Error e -> failwith $"Unexpected error: {e}"

    let createFecha day =
        FechaPago.create day |> unwrapResult

    let createPago () =
        Pago.create None 1000m (createFecha 10) |> unwrapResult

    [<Fact>]
    let ``Create returns Error when cedula is empty`` () =
        let pago = createPago ()

        let result =
            Inquilino.create
                "Juan"
                "Perez"
                ""
                "8090000000"
                pago

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``Create creates inquilino with Deleted false`` () =
        let pago = createPago ()

        let result =
            Inquilino.create
                "Juan"
                "Perez"
                "001-0000000-1"
                "8090000000"
                pago

        match result with
        | Ok inquilino ->
            Assert.False(inquilino.Deleted)
            Assert.Equal("Juan", inquilino.FirstName)
            Assert.Equal("Perez", inquilino.LastName)
            Assert.Equal("001-0000000-1", inquilino.Cedula)
            Assert.Equal("8090000000", inquilino.Telefono)
            Assert.Equal(pago, inquilino.Pago)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``MarkDeleted sets Deleted true and deletes Pago`` () =
        let pago = createPago ()

        let inquilino =
            Inquilino.create
                "Juan"
                "Perez"
                "001-0000000-1"
                "8090000000"
                pago
            |> unwrapResult

        let deleted = Inquilino.markDeleted inquilino

        Assert.True(deleted.Deleted)
        Assert.Equal(EstadoPago.Eliminado, deleted.Pago.Estado)

    [<Fact>]
    let ``Update returns Error when cedula is empty`` () =
        let pago = createPago ()

        let inquilino =
            Inquilino.create
                "Juan"
                "Perez"
                "001-0000000-1"
                "8090000000"
                pago
            |> unwrapResult

        let result =
            Inquilino.update
                "Pedro"
                "Gomez"
                ""
                "8091111111"
                inquilino

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``Update updates fields and keeps Deleted false`` () =
        let pago = createPago ()

        let inquilino =
            Inquilino.create
                "Juan"
                "Perez"
                "001-0000000-1"
                "8090000000"
                pago
            |> unwrapResult

        let result =
            Inquilino.update
                "Pedro"
                "Gomez"
                "002-0000000-2"
                "8091111111"
                inquilino

        match result with
        | Ok updated ->
            Assert.Equal("Pedro", updated.FirstName)
            Assert.Equal("Gomez", updated.LastName)
            Assert.Equal("002-0000000-2", updated.Cedula)
            Assert.Equal("8091111111", updated.Telefono)
            Assert.False(updated.Deleted)
        | Error _ ->
            Assert.True(false)