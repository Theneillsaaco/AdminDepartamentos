using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;

namespace AdminDepartamentos.API.Services.BackgroundServices;

public class CheckRetrasosService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();
                var pagos = await pagoRepository.GetPago();

                List<Pago> pagosUpdate = [];

                foreach (var pago in pagos.Select(pagoInquilinoModel => pagoInquilinoModel.ConvertToPagoEntity()))
                {
                    try
                    {
                        pagoRepository.CheckRetraso(pago);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error checking retraso for pago: {ex.Message}");
                        continue;
                    }

                    pagosUpdate.Add(pago);
                }

                if (pagosUpdate.Count > 0)
                {
                    using var updateScope = scopeFactory.CreateScope();
                    var updateRepo = updateScope.ServiceProvider.GetRequiredService<IPagoRepository>();
                    await updateRepo.Update(pagosUpdate);
                }
            }

            Console.WriteLine("Ejecutando");

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}