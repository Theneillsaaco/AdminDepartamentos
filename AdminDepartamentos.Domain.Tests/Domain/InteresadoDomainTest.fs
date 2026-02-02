namespace AdminDepartamentos.Domain.Tests.Domain

open Xunit
open AdminDepartamentos.Domain.FSharp.Entities
open AdminDepartamentos.Domain.FSharp.ValueObjects

module InteresadoDomainTest =

    let validTipo = Apartamento

    let createInteresado () =
        match Interesado.create "Juan" "Perez" "8091234567" validTipo with
        | Ok i -> i
        | Error e -> failwith $"Error creando interesado: {e}"

    [<Fact>]
    let ``create returns Error when FirstName is empty`` () =
        let result = Interesado.create "" "Perez" "8091234567" validTipo

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``create returns Error when LastName is empty`` () =
        let result = Interesado.create "Juan" "" "8091234567" validTipo

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``create returns Error when Telefono is empty`` () =
        let result = Interesado.create "Juan" "Perez" "" validTipo

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``create returns Ok when data is valid`` () =
        let result = Interesado.create "Juan" "Perez" "8091234567" validTipo

        match result with
        | Ok i ->
            Assert.Equal("Juan", i.FirstName)
            Assert.Equal("Perez", i.LastName)
            Assert.Equal("8091234567", i.Telefono)
            Assert.Equal(validTipo, i.TipoUnidadHabitacional)
            Assert.False(i.Deleted)
        | Error _ -> Assert.True(false)

    [<Fact>]
    let ``update returns Error when interesado is deleted`` () =
        let i = createInteresado () |> Interesado.markDeleted

        let result = Interesado.update "Pedro" "Gomez" "8090001111" Local i

        match result with
        | Error _ -> Assert.True(true)
        | Ok _ -> Assert.True(false)

    [<Fact>]
    let ``update returns Error when fields are empty`` () =
        let i = createInteresado ()

        let r1 = Interesado.update "" "Gomez" "8090001111" Local i
        let r2 = Interesado.update "Pedro" "" "8090001111" Local i
        let r3 = Interesado.update "Pedro" "Gomez" "" Local i

        match r1, r2, r3 with
        | Error _, Error _, Error _ -> Assert.True(true)
        | _ -> Assert.True(false)

    [<Fact>]
    let ``update updates interesado when valid`` () =
        let i = createInteresado ()

        let result = Interesado.update "Pedro" "Gomez" "8090001111" Local i

        match result with
        | Ok updated ->
            Assert.Equal("Pedro", updated.FirstName)
            Assert.Equal("Gomez", updated.LastName)
            Assert.Equal("8090001111", updated.Telefono)
            Assert.Equal(Local, updated.TipoUnidadHabitacional)
        | Error _ -> Assert.True(false)

    [<Fact>]
    let ``markDeleted sets Deleted true`` () =
        let i = createInteresado ()

        let deleted = Interesado.markDeleted i

        Assert.True(deleted.Deleted)

    [<Fact>]
    let ``markDeleted is idempotent`` () =
        let i = createInteresado ()
        let deleted = Interesado.markDeleted i
        let deletedAgain = Interesado.markDeleted deleted

        Assert.True(deletedAgain.Deleted)
