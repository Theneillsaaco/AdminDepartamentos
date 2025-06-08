using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Exceptions;
using AdminDepartamentos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class InquilinoRepositoryTest
{
    [Fact]
    public async Task GetInquilinos_ReturnsNonDeletedInquilinos()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        context.Inquilinos.AddRange(
            new Inquilino { FirstName = "Pepe", LastName = "Dominges", Cedula = "123", Telefono = "829-000-0000", Deleted = false },
            new Inquilino { FirstName = "Juan", LastName = "Fernades", Cedula = "1234", Telefono = "829-000-0000", Deleted = true }
        );
        await context.SaveChangesAsync();
    
        var repo = new InquilinoRepository(context);

        // Act
        var result = await repo.GetInquilinos();
    
        // Assert
        Assert.Single(result);
        Assert.Equal("Pepe", result.First().FirstName);
    }

    [Fact]
    public async Task GetById_ThrowsIfIdInvalid()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var respo = new InquilinoRepository(context);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => respo.GetById(0));
    }

    [Fact]
    public async Task GetById_ThrowsIfNotExists()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Assert
        await Assert.ThrowsAsync<InquilinoException>(() => repo.GetById(999));
    }
    
    [Fact]
    public async Task Save_ThrowsException_WhenCedulaAlreadyExists()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Act
        var inquilinoDto1 =  new InquilinoDto { Cedula = "123", FirstName = "Pepe", LastName = "Dominges" };
        var pagoDto1 = new PagoDto { Monto = 1000, NumDeposito = 1234, FechaPagoInDays = 5 };
        await repo.Save(inquilinoDto1, pagoDto1);
        
        var inquilinoDto2 = new InquilinoDto { Cedula = "123", FirstName = "Otro", LastName = "Inquilino" };
        var pagoDto2 = new PagoDto { Monto = 800, NumDeposito = 6789, FechaPagoInDays = 4 };

        var result = await repo.Save(inquilinoDto2, pagoDto2);
        
        // Assert
        Assert.False(result.Success);
        Assert.Contains("Ocurrio un error al crear el inquilino y el pago. Error:", result.Message);
    }

    [Fact]
    public async Task Save_ThrowsException_WhenDtoIsNull()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.Save(null, new PagoDto()));
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.Save(new InquilinoDto(), null));
    }

    [Fact]
    public async Task Save_AddsInquilinoAndPago()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Act
        var inquilinoDto = new InquilinoDto { FirstName = "Pepe", LastName = "Dominges", Cedula = "123", NumTelefono = "829-000-0000"};
        var pagoDto = new PagoDto { Monto = 500, NumDeposito = 321};
        
        var result = await repo.Save(inquilinoDto, pagoDto);
        
        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, context.Inquilinos.Count());
        Assert.Equal(1, context.Pagos.Count());
    }
    
    [Fact]
    public async Task Update_ThrowsException_WhenInquilinoIsNull()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.Update((Inquilino)null));
    }
    
    [Fact]
    public async Task Update_UpdatesEntityCorrectly()
    {
        // Arrange
        await using var context = new DepartContext(_options);

        var existing = new Inquilino { IdInquilino = 1, Cedula = "123", FirstName = "Pepe", LastName = "Dominges", Telefono = "829-000-0000"};
        context.Inquilinos.Add(existing);
        await context.SaveChangesAsync();
        
        var repo = new InquilinoRepository(context);

        // Act
        existing.FirstName = "Pedro";
        await repo.Update(existing);

        // Assert
        var updated = await context.Inquilinos.FindAsync(1);
        Assert.Equal("Pedro", updated.FirstName);
        Assert.Equal(1, context.Inquilinos.Count());
    }
    
    [Fact]
    public async Task UpdateMultipleEntities_UpdatesAllCorrectly()
    {
        // Arrange
        await using var context = new DepartContext(_options);

        var inq1 = new Inquilino { IdInquilino = 1, Cedula = "001", FirstName = "Ana", LastName = "Martines", Telefono = "829-000-0000"};
        var inq2 = new Inquilino { IdInquilino = 2, Cedula = "002", FirstName = "Luis", LastName = "Francisco", Telefono = "829-000-0000"};
        context.Inquilinos.AddRange(inq1, inq2);
        await context.SaveChangesAsync();

        var repo = new InquilinoRepository(context);

        // Act
        inq1.FirstName = "Andrea";
        inq2.FirstName = "Lucas";

        await repo.Update(new List<Inquilino> { inq1, inq2 }); // Llama al método de base

        var updated1 = await context.Inquilinos.FindAsync(1);
        var updated2 = await context.Inquilinos.FindAsync(2);

        // Assert
        Assert.Equal("Andrea", updated1.FirstName);
        Assert.Equal("Lucas", updated2.FirstName);
        Assert.Equal(2, context.Inquilinos.Count());
    }
    
    [Fact]
    public async Task MarkDeleted_SetsDeletedFlags()
    {
        // Arrange
        await using var context = new DepartContext(_options);
        var repo = new InquilinoRepository(context);
        
        // Act
        var inquilinoDto = new InquilinoDto { FirstName = "Pepe", LastName = "Dominges", Cedula = "123", NumTelefono = "829-000-0000"};
        var pagoDto = new PagoDto { Monto = 500, NumDeposito = 321};
        
        await repo.Save(inquilinoDto, pagoDto);
        await repo.MarkDeleted(1);
        
        var inq = await context.Inquilinos.FindAsync(1);
        
        // Assert
        Assert.True(inq.Deleted);
        Assert.True(context.Pagos.First().Deleted);
    }
    
    #region Arrange
    
    private readonly DbContextOptions<DepartContext> _options;

    public InquilinoRepositoryTest()
    {
        _options = new DbContextOptionsBuilder<DepartContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    #endregion
}