using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Infrastucture.Context;
using AdminDepartamentos.Infrastucture.Context.Entities;
using AdminDepartamentos.Infrastucture.Exceptions;
using AdminDepartamentos.Infrastucture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminDepartamentos.Unit.Test.Infrastructure;

public class PagoRepositoryTest
{
    [Fact]
    public async Task GetById_Throws_WhenIdInvalid()
    {
        await using var context = new DepartContext(_options);
        var repo = new PagoRepository(context);

        await Assert.ThrowsAsync<ArgumentException>(() => repo.GetById(0));
    }

    [Fact]
    public async Task GetById_Throws_WhenNotFound()
    {
        await using var context = new DepartContext(_options);
        var repo = new PagoRepository(context);

        await Assert.ThrowsAsync<PagoException>(() => repo.GetById(999));
    }

    [Fact]
    public async Task GetById_ReturnsPago_WhenExists()
    {
        await using var context = new DepartContext(_options);
        var pago = new PagoEntity { IdPago = 1, IdInquilino = 1, Monto = 100, NumDeposito = 50 };
        context.Pagos.Add(pago);
        await context.SaveChangesAsync();

        var repo = new PagoRepository(context);
        var result = await repo.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdPago);
    }
    
    #region Arrange

    private readonly DbContextOptions<DepartContext> _options;

    public PagoRepositoryTest()
    {
        _options = new DbContextOptionsBuilder<DepartContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }

    #endregion
}