using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Models.InteresadoModels;
using AdminDepartamentos.Infrastructure.Repositories;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class InteresadoRepositoryTest
{
    #region GetById

    [Fact]
    public async Task GetById_Returns_Interesado_When_Exists()
    {
        // Arrange
        var entity = new InteresadoEntity
        {
            IdInteresado = 1,
            FirstName = "Pedro",
            LastName = "Gomez",
            Telefono = "8091111111",
            TipoUnidadHabitacional = "Apartamento"
        };

        _context.Interesados.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pedro", result.FirstName);
    }

    [Fact]
    public async Task GetById_Throws_When_Id_Is_Invalid()
    {
        // Act
        Func<Task> act = async () => await _repository.GetById(0);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task GetById_Throws_When_Not_Found()
    {
        // Act
        Func<Task> act = async () => await _repository.GetById(999);

        // Assert
        await Assert.ThrowsAsync<InquilinoException>(act);
    }

    #endregion

    #region GetByType

    [Fact]
    public async Task GetByType_Returns_Only_Matching_Type()
    {
        // Arrange
        _context.Interesados.AddRange(
            CreateInteresado(1, "Apartamento", firstName: "Juan"),
            CreateInteresado(2, "Local", firstName: "Maria")
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByType("Apartamento");

        // Assert
        Assert.Single(result);
        Assert.Equal("Juan", result[0].FirstName);
    }

    [Fact]
    public async Task GetByType_Respects_LastId_And_Take()
    {
        // Arrange
        _context.Interesados.AddRange(
            CreateInteresado(1),
            CreateInteresado(2)
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByType("Apartamento", lastId: 1, take: 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].IdInteresado);
    }

    #endregion

    #region GetPendingInteresado

    [Fact]
    public async Task GetPendingInteresado_Returns_Ordered_And_Limited_List()
    {
        // Arrange
        _context.Interesados.AddRange(
            CreateInteresado(1, fecha: DateTime.UtcNow.AddDays(-3)),
            CreateInteresado(2, fecha: DateTime.UtcNow.AddDays(-2)),
            CreateInteresado(3, fecha: DateTime.UtcNow.AddDays(-1))
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPendingInteresado(take: 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].IdInteresado);
    }

    #endregion

    #region Save

    [Fact]
    public async Task Save_Returns_Success_When_Data_Is_Valid()
    {
        // Arrange
        var dto = new InteresadoDto
        {
            FirstName = "Luis",
            LastName = "Perez",
            Telefono = "8092222222",
            TipoUnidadHabitacional = "Apartamento"
        };

        // Act
        var result = await _repository.Save(dto);

        // Assert
        Assert.True(result.Success);
        Assert.Single(_context.Interesados);
    }

    [Fact]
    public async Task Save_Returns_False_When_Dto_Is_Null()
    {
        // Act
        var result = await _repository.Save(null!);

        // Assert
        Assert.False(result.Success);
    }

    #endregion

    #region UpdateInteresado

    [Fact]
    public async Task UpdateInteresado_Updates_Data_When_Exists()
    {
        // Arrange
        var entity = new InteresadoEntity
        {
            IdInteresado = 1,
            FirstName = "Old",
            LastName = "Name",
            Telefono = "000",
            TipoUnidadHabitacional = "Apartamento"
        };

        _context.Interesados.Add(entity);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateInteresado(
            1,
            "New",
            "Name",
            "999",
            "Local"
        );

        // Assert
        var updated = await _context.Interesados.FindAsync(1);
        Assert.Equal("New", updated!.FirstName);
        Assert.Equal("Local", updated.TipoUnidadHabitacional);
    }

    #endregion

    #region MarkDeleted

    [Fact]
    public async Task MarkDeleted_Marks_Entity_As_Deleted()
    {
        // Arrange
        _context.Interesados.Add(CreateInteresado(1));
        await _context.SaveChangesAsync();

        // Act
        await _repository.MarkDeleted(1);

        // Assert
        var deleted = await _context.Interesados.FindAsync(1);
        Assert.True(deleted!.Deleted);
    }

    #endregion

    private static InteresadoEntity CreateInteresado(int id, string tipo = "Apartamento", DateTime? fecha = null, string firstName = "Test")
    {
        return new InteresadoEntity
        {
            IdInteresado = id,
            FirstName = firstName,
            LastName = $"User{id}",
            Telefono = "8090000000",
            TipoUnidadHabitacional = tipo,
            Fecha = fecha ?? DateTime.UtcNow,
            Deleted = false
        };
    }


    #region Fields

    private readonly DepartContext _context;
    private readonly InteresadoRepository _repository;

    public InteresadoRepositoryTest()
    {
        _context = Context.DbContextFactory.Create();
        _repository = new InteresadoRepository(_context);
    }

    #endregion
}
