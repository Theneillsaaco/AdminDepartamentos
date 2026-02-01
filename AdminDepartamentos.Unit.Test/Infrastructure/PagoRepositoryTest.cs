
using AdminDepartamentos.Domain.FSharp.Entities;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Repositories;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class PagoRepositoryTest
{
    #region GetById

    [Fact]
    public async Task GetById_Returns_Pago_When_Exists()
    {
        // Arrange
        var inquilino = new InquilinoEntity 
        { 
            IdInquilino = 1, 
            FirstName = "Juan", 
            LastName = "Perez", 
            Cedula = "12345678", 
            Telefono = "555-1234" 
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
        var result = await _repository.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123456, result.NumDeposito);
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
        await Assert.ThrowsAsync<PagoException>(act);
    }

    #endregion

    #region GetPago

    [Fact]
    public async Task GetPago_Returns_List_With_Inquilino_Info()
    {
        // Arrange
        var inquilino = new InquilinoEntity 
        { 
            IdInquilino = 1, 
            FirstName = "Juan", 
            LastName = "Perez", 
            Cedula = "12345678", 
            Telefono = "555-1234" 
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
        var result = await _repository.GetPago(take: 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(123456, result[0].NumDeposito);
        Assert.Equal("Juan", result[0].InquilinoFirstName);
    }

    [Fact]
    public async Task GetPago_Respects_LastId_And_Take()
    {
        // Arrange
        _context.Inquilinos.AddRange(
            new InquilinoEntity { IdInquilino = 1, FirstName = "Juan", LastName = "Perez", Cedula = "12345678", Telefono = "555-1234" },
            new InquilinoEntity { IdInquilino = 2, FirstName = "Maria", LastName = "Gomez", Cedula = "87654321", Telefono = "555-5678" },
            new InquilinoEntity { IdInquilino = 3, FirstName = "Luis", LastName = "Lopez", Cedula = "11223344", Telefono = "555-9012" }
        );

        _context.Pagos.AddRange(
            new PagoEntity { IdPago = 1, IdInquilino = 1, NumDeposito = 1, Monto = 100, FechaPagoInDays = 10 },
            new PagoEntity { IdPago = 2, IdInquilino = 2, NumDeposito = 2, Monto = 200, FechaPagoInDays = 20 },
            new PagoEntity { IdPago = 3, IdInquilino = 3, NumDeposito = 3, Monto = 300, FechaPagoInDays = 30 }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPago(lastId: 1, take: 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(2, result[0].IdPago);
    }

    #endregion

    #region Retrasos

    [Fact]
    public async Task GetRetrasosWithoutEmail_Returns_Only_Retrasos_Without_Email()
    {
        // Arrange
        _context.Inquilinos.AddRange(
            new InquilinoEntity { IdInquilino = 1, FirstName = "Juan", LastName = "Perez", Cedula = "12345678", Telefono = "555-1234" },
            new InquilinoEntity { IdInquilino = 2, FirstName = "Maria", LastName = "Gomez", Cedula = "87654321", Telefono = "555-5678" }
        );

        _context.Pagos.AddRange(
            new PagoEntity { IdPago = 1, IdInquilino = 1, NumDeposito = 1, Monto = 100, FechaPagoInDays = 30, Email = false, Retrasado = true },
            new PagoEntity { IdPago = 2, IdInquilino = 2, NumDeposito = 2, Monto = 200, FechaPagoInDays = 30, Email = true, Retrasado = true }
        );
        
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRetrasosWithoutEmail();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].IdPago);
        Assert.False(result[0].Email);
        Assert.True(result[0].Retrasado);
    }

    #endregion

    #region UpdatePago

    [Fact]
    public async Task UpdatePago_Updates_Fields_When_Exists()
    {
        // Arrange
        var inquilino = new InquilinoEntity
        {
            IdInquilino = 1,
            FirstName = "Ana",
            LastName = "Diaz",
            Cedula = "005",
            Telefono = "8094444444"
        };

        var pago = new PagoEntity
        {
            IdPago = 1,
            IdInquilino = 1,
            NumDeposito = 111,
            Monto = 500,
            FechaPagoInDays = 15
        };

        _context.Inquilinos.Add(inquilino);
        _context.Pagos.Add(pago);
        await _context.SaveChangesAsync();

        var updatedPago = new PagoEntity
        {
            NumDeposito = 999,
            Monto = 1500,
            FechaPagoInDays = 30
        };

        // Act
        var result = await _repository.UpdatePago(1, updatedPago);

        // Assert
        Assert.True(result);

        var pagoDb = await _context.Pagos.FindAsync(1);
        Assert.Equal(999, pagoDb!.NumDeposito);
        Assert.Equal(1500, pagoDb.Monto);
    }

    [Fact]
    public async Task UpdatePago_Returns_False_When_Not_Found()
    {
        // Act
        var result = await _repository.UpdatePago(999, new PagoEntity());

        // Assert
        Assert.False(result);
    }


    #endregion

    #region Fields

    private readonly DepartContext _context;
    private readonly PagoRepository _repository;

    public PagoRepositoryTest()
    {
        _context = Context.DbContextFactory.Create();
        _repository = new PagoRepository(_context);
    }

    #endregion
}