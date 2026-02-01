using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Models.UnidadHabitacionalModels;
using AdminDepartamentos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class UnidadHabitacionalRepositoryTest
{
    #region GetUnidadHabitacionales

    [Fact]
    public async Task GetUnidadHabitacionales_Returns_List()
    {
        // Arrange
        _context.UnidadHabitacionals.AddRange(
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 1,
                Name = "A-01",
                Tipo = "Apartamento",
                LightCode = "L1"
            },
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 2,
                Name = "A-02",
                Tipo = "Apartamento",
                LightCode = "L2"
            }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUnidadHabitacionales();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUnidadHabitacionales_Returns_Empty_When_None()
    {
        // Act
        var result = await _repository.GetUnidadHabitacionales();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnidadHabitacionales_Respects_LastId_And_Take()
    {
        // Arrange
        _context.UnidadHabitacionals.AddRange(
            new UnidadHabitacionalEntity { IdUnidadHabitacional = 1, Name = "A1", Tipo = "Apartamento", LightCode = "L1" },
            new UnidadHabitacionalEntity { IdUnidadHabitacional = 2, Name = "A2", Tipo = "Apartamento", LightCode = "L2" },
            new UnidadHabitacionalEntity { IdUnidadHabitacional = 3, Name = "A3", Tipo = "Apartamento", LightCode = "L3" }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUnidadHabitacionales(lastId: 1, take: 1);

        // Assert
        Assert.Single(result);
        Assert.True(result[0].IdUnidadHabitacional > 1);
    }

    #endregion

    #region GetAvailableUnidadHabitacional

    [Fact]
    public async Task GetAvailableUnidadHabitacional_Returns_Only_Free_Units()
    {
        // Arrange
        _context.UnidadHabitacionals.AddRange(
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 1,
                Name = "Libre",
                Tipo = "Apartamento",
                LightCode = "L1",
                IdInquilinoActual = null
            },
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 2,
                Name = "Ocupada",
                Tipo = "Apartamento",
                LightCode = "L2",
                IdInquilinoActual = 1
            }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAvailableUnidadHabitacional();

        // Assert
        Assert.Single(result);
        Assert.Null(result[0].IdInquilinoActual);
    }

    #endregion

    #region GetOccupiedUnidadHabitacional

    [Fact]
    public async Task GetOccupiedUnidadHabitacional_Returns_Only_Occupied()
    {
        // Arrange
        _context.UnidadHabitacionals.AddRange(
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 1,
                Name = "Libre",
                Tipo = "Apartamento",
                LightCode = "L1"
            },
            new UnidadHabitacionalEntity
            {
                IdUnidadHabitacional = 2,
                Name = "Ocupada",
                Tipo = "Apartamento",
                LightCode = "L2",
                IdInquilinoActual = 1
            }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOccupiedUnidadHabitacional();

        // Assert
        Assert.Single(result);
        Assert.NotNull(result[0].IdInquilinoActual);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_Returns_Unidad_When_Exists()
    {
        // Arrange
        var unidad = new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = 1,
            Name = "A-01",
            Tipo = "Apartamento",
            LightCode = "L1"
        };

        _context.UnidadHabitacionals.Add(unidad);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("A-01", result.Name);
    }

    [Fact]
    public async Task GetById_Throws_When_Id_Invalid()
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

    #region Save

    [Fact]
    public async Task Save_Creates_UnidadHabitacional()
    {
        // Arrange
        var dto = new UnidadHabitacionalDto
        {
            Name = "A-100",
            Tipo = "Apartamento",
            LightCode = "L100"
        };

        // Act
        var result = await _repository.Save(dto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Unidad Habitacional creada exitosamente.", result.Message);

        var unidad = await _context.UnidadHabitacionals.FirstOrDefaultAsync();
        Assert.NotNull(unidad);
    }

    [Fact]
    public async Task Save_Fails_When_Tipo_Invalid()
    {
        // Arrange
        var dto = new UnidadHabitacionalDto
        {
            Name = "A-200",
            Tipo = "Casa",
            LightCode = "L200"
        };

        // Act
        var result = await _repository.Save(dto);

        // Assert
        Assert.False(result.Success);
    }

    #endregion

    #region Update / Assign / Release / Delete

    [Fact]
    public async Task UpdateUnidadHabitacional_Updates_Fields()
    {
        // Arrange
        var unidad = new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = 1,
            Name = "Old",
            Tipo = "Apartamento",
            LightCode = "L1"
        };

        _context.UnidadHabitacionals.Add(unidad);
        await _context.SaveChangesAsync();

        // Act
        await _repository.UpdateUnidadHabitacional(1, "New", "Apartamento", "L2");

        // Assert
        var updated = await _context.UnidadHabitacionals.FindAsync(1);
        Assert.Equal("New", updated!.Name);
        Assert.Equal("L2", updated.LightCode);
    }

    [Fact]
    public async Task AssignInquilino_Assigns_Inquilino()
    {
        // Arrange
        var unidad = new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = 1,
            Name = "A1",
            Tipo = "Apartamento",
            LightCode = "L1"
        };

        _context.UnidadHabitacionals.Add(unidad);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AssignInquilino(1, 10);

        // Assert
        Assert.True(result);

        var updated = await _context.UnidadHabitacionals.FindAsync(1);
        Assert.Equal(10, updated!.IdInquilinoActual);
    }

    [Fact]
    public async Task ReleaseUnit_Removes_Inquilino()
    {
        // Arrange
        var unidad = new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = 1,
            Name = "A1",
            Tipo = "Apartamento",
            LightCode = "L1",
            IdInquilinoActual = 5
        };

        _context.UnidadHabitacionals.Add(unidad);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ReleaseUnit(1);

        // Assert
        Assert.True(result);

        var updated = await _context.UnidadHabitacionals.FindAsync(1);
        Assert.Null(updated!.IdInquilinoActual);
    }

    [Fact]
    public async Task MarkDeleted_Marks_Unidad_As_Deleted()
    {
        // Arrange
        var unidad = new UnidadHabitacionalEntity
        {
            IdUnidadHabitacional = 1,
            Name = "A1",
            Tipo = "Apartamento",
            LightCode = "L1"
        };

        _context.UnidadHabitacionals.Add(unidad);
        await _context.SaveChangesAsync();

        // Act
        await _repository.MarkDeleted(1);

        // Assert
        var deleted = await _context.UnidadHabitacionals.FindAsync(1);
        Assert.True(deleted!.Deleted);
    }

    #endregion

    #region Fields

    private readonly DepartContext _context;
    private readonly UnidadHabitacionalRepository _repository;

    public UnidadHabitacionalRepositoryTest()
    {
        _context = Context.DbContextFactory.Create();
        _repository = new UnidadHabitacionalRepository(_context);
    }

    #endregion
}
