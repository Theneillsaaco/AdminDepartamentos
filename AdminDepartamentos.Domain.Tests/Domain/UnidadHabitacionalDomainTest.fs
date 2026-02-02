namespace AdminDepartamentos.Domain.Tests.Domain

open Xunit
open AdminDepartamentos.Domain.FSharp.Entities
open AdminDepartamentos.Domain.FSharp.ValueObjects

module UnidadHabitacionalDomainTest =

    let validTipo = Apartamento

    let createUnidad () =
        match UnidadHabitacional.create "A-101" validTipo "LC-001" with
        | Ok u -> u
        | Error e -> failwith $"Error creando unidad: {e}"

    [<Fact>]
    let ``create returns Error when name is empty`` () =
        let result = UnidadHabitacional.create "" validTipo "LC-001"

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``create returns Error when lightCode is empty`` () =
        let result = UnidadHabitacional.create "A-101" validTipo ""

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``create creates unidad with Libre state`` () =
        let result = UnidadHabitacional.create "A-101" validTipo "LC-001"

        match result with
        | Ok unidad ->
            Assert.Equal("A-101", unidad.Name)
            Assert.Equal(validTipo, unidad.Tipo)
            Assert.Equal("LC-001", unidad.LightCode)
            Assert.Equal(EstadoUnidad.Libre, unidad.Estado)
            Assert.False(unidad.Deleted)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``updateInfo updates unidad info when valid`` () =
        let unidad = createUnidad ()

        let result =
            UnidadHabitacional.updateInfo "B-202" Local "LC-999" unidad

        match result with
        | Ok updated ->
            Assert.Equal("B-202", updated.Name)
            Assert.Equal(Local, updated.Tipo)
            Assert.Equal("LC-999", updated.LightCode)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``updateInfo returns Error when unidad is deleted`` () =
        let unidad =
            createUnidad ()
            |> UnidadHabitacional.markDeleted

        let result =
            UnidadHabitacional.updateInfo "B-202" Local "LC-999" unidad

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``assingInquilino assigns inquilino when unidad is Libre`` () =
        let unidad = createUnidad ()

        let result = UnidadHabitacional.assingInquilino 10 unidad

        match result with
        | Ok updated ->
            match updated.Estado with
            | Ocupada id -> Assert.Equal(10, id)
            | _ -> Assert.True(false)
        | Error _ ->
            Assert.True(false)

    [<Fact>]
    let ``assingInquilino returns Error when unidad is Ocupada`` () =
        let unidad =
            match UnidadHabitacional.assingInquilino 10 (createUnidad ()) with
            | Ok u -> u
            | Error e -> failwith $"Unexpected error: {e}"

        let result = UnidadHabitacional.assingInquilino 20 unidad

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``assingInquilino returns Error when unidad is deleted`` () =
        let unidad =
            createUnidad ()
            |> UnidadHabitacional.markDeleted

        let result = UnidadHabitacional.assingInquilino 10 unidad

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``release changes Ocupada to Libre`` () =
        let unidad =
            createUnidad ()
            |> UnidadHabitacional.assingInquilino 10
            |> function
                | Ok u -> u
                | Error e -> failwith $"Unexpected error: {e}"

        let released = UnidadHabitacional.release unidad

        Assert.Equal(EstadoUnidad.Libre, released.Estado)

    [<Fact>]
    let ``release keeps Libre when already Libre`` () =
        let unidad = createUnidad ()

        let released = UnidadHabitacional.release unidad

        Assert.Equal(EstadoUnidad.Libre, released.Estado)

    [<Fact>]
    let ``markDeleted sets Deleted true and releases unidad`` () =
        let unidad =
            createUnidad ()
            |> UnidadHabitacional.assingInquilino 10
            |> function
                | Ok u -> u
                | Error e -> failwith $"Unexpected error: {e}"

        let deleted = UnidadHabitacional.markDeleted unidad

        Assert.True(deleted.Deleted)
        Assert.Equal(EstadoUnidad.Libre, deleted.Estado)

    [<Fact>]
    let ``markDeleted is idempotent`` () =
        let unidad =
            createUnidad ()
            |> UnidadHabitacional.markDeleted

        let deletedAgain = UnidadHabitacional.markDeleted unidad

        Assert.True(deletedAgain.Deleted)
