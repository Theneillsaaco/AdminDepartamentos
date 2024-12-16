using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastructure.Context;

namespace AdminDepartamentos.API.Services.BackgroundServices;

public class CheckRetrasosService : BackgroundService
{
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Ejecutando CheckRetrasosService");
                await ProcessRetraso(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CheckRetrasosService: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task ProcessRetraso(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();
        var dbContext = scope.ServiceProvider.GetRequiredService<DepartContext>();
        
        var pagos = await pagoRepository.GetPago();
        if (!pagos.Any())
            return;
        
        var pagosUpdate = new List<Pago>();

        foreach (var pago in pagos.Select(pagoInquilinoModel => pagoInquilinoModel.ConvertToPagoEntity()))
        {
            try
            {
                if (!UpdateRetrasoStatus(pago, pagoRepository)) continue;
                
                pagosUpdate.Add(pago);
                Console.WriteLine($"Marked Pago ID {pago.IdPago} as retrasado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking retraso for Pago ID {pago.IdPago}: {ex.Message}");
            }
        }

        if (pagosUpdate.Any())
            await UpdatePagos(pagosUpdate, dbContext, stoppingToken);
    }
    
    private bool UpdateRetrasoStatus(Pago pago, IPagoRepository pagoRepository)
    {
        bool originalState = pago.Retrasado;
        pagoRepository.CheckRetraso(pago);
        return pago.Retrasado != originalState;
    }
    
    private async Task UpdatePagos(List<Pago> pagosUpdate, DepartContext dbContext, CancellationToken stoppingToken)
    {
        using var updateScope = _scopeFactory.CreateScope();
        var pagoRepository = updateScope.ServiceProvider.GetRequiredService<IPagoRepository>();

        try
        {
            await pagoRepository.Update(pagosUpdate);
            await dbContext.SaveChangesAsync(stoppingToken);
            Console.WriteLine($"{pagosUpdate.Count} pagos actualizados en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar los pagos: {ex.Message}");
        }
    }
    
    #region Context

    private readonly IServiceScopeFactory _scopeFactory;
    
    public CheckRetrasosService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    #endregion
}