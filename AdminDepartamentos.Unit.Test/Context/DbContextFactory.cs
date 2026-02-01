using AdminDepartamentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminDepartamentos.Unit.Test.Context;

public class DbContextFactory
{
    public static DepartContext Create()
    {
        var options = new DbContextOptionsBuilder<DepartContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;


        return new DepartContext(options);
    }
}
