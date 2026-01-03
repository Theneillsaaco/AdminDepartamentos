using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Infrastructure.Context;

namespace AdminDepartamentos.API.Services.BackgroundServices;

public class CheckRetrasosService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CheckRetrasosService background service started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            using (_logger.BeginScope("CheckRetrasosServiceCycle {CycleId}", Guid.NewGuid()))
            {
                try
                {
                    _logger.LogInformation("Starting daily retraso check.");
                    await ProcessRetraso(stoppingToken);
                    _logger.LogInformation("Daily retraso check finished successfully.");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("CheckRetrasosService cancellation requested.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking retraso.");
                }
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        _logger.LogInformation("CheckRetrasosService stopped.");
    }

    private async Task ProcessRetraso(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();
        var dbContext = scope.ServiceProvider.GetRequiredService<DepartContext>();

        var pagos = await pagoRepository.GetPago();

        _logger.LogInformation("Retrieved {Count} pagos for retraso evaluation.", pagos.Count());

        if (!pagos.Any())
            return;

        var pagosUpdate = new List<Pago>();

        foreach (var pago in pagos.Select(pagoInquilinoModel => pagoInquilinoModel.ConvertToPagoEntity()))
            using (_logger.BeginScope("PagoId {IdPago}", pago.IdPago))
            {
                try
                {
                    if (!UpdateRetrasoStatus(pago, pagoRepository))
                    {
                        _logger.LogDebug("Pago retraso status unchanged.");
                        continue;
                    }

                    pagosUpdate.Add(pago);
                    _logger.LogInformation("Pago marked as retrasado.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking retraso for pago.");
                }
            }

        if (pagosUpdate.Any())
            await UpdatePagos(pagosUpdate, dbContext, stoppingToken);
    }

    private bool UpdateRetrasoStatus(Pago pago, IPagoRepository pagoRepository)
    {
        var originalState = pago.Retrasado;
        pagoRepository.CheckRetraso(pago);
        return pago.Retrasado != originalState;
    }

    private async Task UpdatePagos(List<Pago> pagosUpdate, DepartContext dbContext, CancellationToken stoppingToken)
    {
        using var updateScope = _scopeFactory.CreateScope();
        var pagoRepository = updateScope.ServiceProvider.GetRequiredService<IPagoRepository>();

        try
        {
            _logger.LogInformation("Updating {Count} pagos with retraso changes.",
                pagosUpdate.Count
            );

            await pagoRepository.Update(pagosUpdate);
            await dbContext.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("Pagos updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating pagos retraso status.");
        }
    }

    #region Fields

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CheckRetrasosService> _logger;

    public CheckRetrasosService(IServiceScopeFactory scopeFactory, ILogger<CheckRetrasosService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    #endregion
}