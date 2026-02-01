using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Models.PagoModels;
using AdminDepartamentos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class InquilinoRepositoryTest
{
    #region GetInquilinos

    [Fact]
    public async Task GetInquilinos_Returns_Limited_List()
    {
        // Arrange
        _context.Inquilinos.AddRange(
        
            new InquilinoEntity { IdInquilino = 1, FirstName = "Inquilino 1", LastName = "Apellido 1", Cedula = "123456", Telefono = "555-1234" },
            new InquilinoEntity { IdInquilino = 2, FirstName = "Inquilino 2", LastName = "Apellido 2", Cedula = "789012", Telefono = "555-5678" },
            new InquilinoEntity { IdInquilino = 3, FirstName = "Inquilino 3", LastName = "Apellido 3", Cedula = "345678", Telefono = "555-9012" }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetInquilinos(take: 2);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetInquilinos_Returns_Empty_List_When_NoneExist()
    {
        // Act
        var result = await _repository.GetInquilinos(take: 2);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetInquilinos_Respects_LastId()
    {
        // Arrange
        _context.Inquilinos.AddRange(
            new InquilinoEntity { IdInquilino = 1, FirstName = "Inquilino 1", LastName = "Apellido 1", Cedula = "123456", Telefono = "555-1234" },
            new InquilinoEntity { IdInquilino = 2, FirstName = "Inquilino 2", LastName = "Apellido 2", Cedula = "789012", Telefono = "555-5678" },
            new InquilinoEntity { IdInquilino = 3, FirstName = "Inquilino 3", LastName = "Apellido 3", Cedula = "345678", Telefono = "555-9012" }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetInquilinos(lastId: 1, take: 2);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, i => Assert.True(i.IdInquilino > 1));
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_Returns_Inquilino_When_Exists()
    {
        // Arrange
        var inquilino = new InquilinoEntity { IdInquilino = 1, FirstName = "Inquilino 1", LastName = "Apellido 1", Cedula = "123456", Telefono = "555-1234" };
        
        _context.Inquilinos.Add(inquilino);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Inquilino 1", result.FirstName);
    }

    [Fact]
    public async Task GetById_Throws_When_Id_Is_Invalid()
    {
        // Act
        Func<Task> act = async () => { await _repository.GetById(0); };

        // Assert
        await Assert.ThrowsAsync<InquilinoException>(act);
    }

    [Fact]
    public async Task GetById_Throws_When_Not_Found()
    {
        // Act
        Func<Task> act = async () => { await _repository.GetById(999); };

        // Assert
        await Assert.ThrowsAsync<InquilinoException>(act);
    }

    #endregion

    #region Save

    [Fact]
    public async Task Save_Creates_Inquilino_And_Pago()
    {
        // Arrange
        var inquilinoDto = new InquilinoDto
        {
            FirstName = "Nuevo",
            LastName = "Inquilino",
            Cedula = "654321",
            NumTelefono = "555-4321"
        };

        var pagoDto = new PagoDto
        {
            Monto = 500,
            NumDeposito = 12345,
            FechaPagoInDays = 30
        };

        // Act
        var result = await _repository.Save(inquilinoDto, pagoDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Inquilino y pago creados exitosamente.", result.Message);
        var createdInquilino = await _context.Inquilinos.FirstOrDefaultAsync(i => i.Cedula == inquilinoDto.Cedula);
        Assert.NotNull(createdInquilino);
    }

    [Fact]
    public async Task Save_Fails_When_Cedula_Exists()
    {
        // Arrange
        _context.Inquilinos.Add(new InquilinoEntity
        {
            FirstName = "Test",
            LastName = "User",
            Cedula = "001-0000000-1",
            Telefono = "0000000000"
        });

        await _context.SaveChangesAsync();

        var inquilinoDto = new InquilinoDto
        {
            FirstName = "Juan",
            LastName = "Perez",
            Cedula = "001-0000000-1",
            NumTelefono = "8090000000"
        };

        var pagoDto = new PagoDto
        {
            NumDeposito = 455356,
            Monto = 15000,
            FechaPagoInDays = 30
        };

        var result = await _repository.Save(inquilinoDto, pagoDto);

        Assert.False(result.Success);
        Assert.Equal("El inquilino ya existe. Por favor, use otro número de cédula.", result.Message);
    }

    [Fact]
    public async Task Save_Does_Not_Create_Pago_When_Fails()
    {
        // Arrange 
        _context.Inquilinos.Add(new InquilinoEntity
        {
            FirstName = "Test",
            LastName = "User",
            Cedula = "001-0000000-1",
            Telefono = "0000000000" 
        });

        await _context.SaveChangesAsync();

        var result = await _repository.Save(
            new InquilinoDto
            {
                FirstName = "Juan",
                LastName = "Perez",
                Cedula = "001-0000000-1",
                NumTelefono = "8090000000"
            },
            new PagoDto
            {
                NumDeposito = 455356,
                Monto = 15000,
                FechaPagoInDays = 30
            }
        );

        var pagos = await _context.Pagos.ToListAsync();

        Assert.False(result.Success);
        Assert.Empty(pagos);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_Updates_Fields()
    {
        // Arrange
        var inquilino = new InquilinoEntity
        {
            IdInquilino = 1,
            FirstName = "Juan",
            LastName = "Perez",
            Cedula = "001-0000000-1",
            Telefono = "8090000000"
        };

        var pago = new PagoEntity
        {
            IdPago = 1,
            IdInquilino = 1,
            NumDeposito = 123456,
            Monto = 10000,
            FechaPagoInDays = 30
        };

        _context.Inquilinos.Add(inquilino);
        _context.Pagos.Add(pago);
        await _context.SaveChangesAsync();

        // Act
        inquilino.FirstName = "Carlos";
        inquilino.Telefono = "8091111111";

        await _repository.UpdateInquilino(1, inquilino);

        // Assert
        var updatedInquilino = await _context.Inquilinos.FindAsync(1);
        Assert.NotNull(updatedInquilino);
        Assert.Equal("Carlos", updatedInquilino.FirstName);
        Assert.Equal("8091111111", updatedInquilino.Telefono);
    }

    [Fact]
    public async Task Update_Throws_When_Inquilino_Not_Found()
    {
        // Arrange
        var inquilino = new InquilinoEntity
        {
            FirstName = "Juan",
            LastName = "Perez",
            Cedula = "001-0000000-1",
            Telefono = "8090000000"
        };

        // Act
        Func<Task> act = async () => { await _repository.UpdateInquilino(999, inquilino); };

        // Assert
        await Assert.ThrowsAsync<InquilinoException>(act);
    }

    #endregion

    [Fact]
    public async Task MarkDeleted_Marks_Inquilino_As_Deleted()
    {
        // Arrange
        var inquilino = new InquilinoEntity
        {
            IdInquilino = 1,
            FirstName = "Pedro",
            LastName = "Gomez",
            Cedula = "002-0000000-2",
            Telefono = "8091111111"
        };

        _context.Inquilinos.Add(inquilino);
        await _context.SaveChangesAsync();

        // Act
        await _repository.MarkDeleted(1);

        // Assert
        var deletedInquilino = await _context.Inquilinos.FindAsync(1);
        Assert.NotNull(deletedInquilino);
        Assert.True(deletedInquilino.Deleted);
    }
    
    #region Fields

    private readonly DepartContext _context;
    private readonly InquilinoRepository _repository;

    public InquilinoRepositoryTest()
    {
        _context = Context.DbContextFactory.Create();
        _repository = new InquilinoRepository(_context);
    }

    #endregion
}