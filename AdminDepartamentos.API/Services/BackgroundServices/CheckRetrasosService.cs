﻿using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Infrastructure.Context;

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
                        bool estadoOriginalRetrasado = pago.Retrasado;
                        
                        pagoRepository.CheckRetraso(pago);

                        if (pago.Retrasado == estadoOriginalRetrasado) continue;
                        pagosUpdate.Add(pago);
                        Console.WriteLine($"Marked Pago ID {pago.IdPago} as retrasado.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error checking retraso for pago: {ex.Message}");
                        continue;
                    }
                }

                if (pagosUpdate.Count > 0)
                {
                    using var updateScope = scopeFactory.CreateScope();
                    var updateRepo = updateScope.ServiceProvider.GetRequiredService<IPagoRepository>();
                    await updateRepo.Update(pagosUpdate);
                    await scope.ServiceProvider.GetRequiredService<DepartContext>().SaveChangesAsync(stoppingToken);
                }
            }

            Console.WriteLine("Ejecutando CheckRetrasosService");

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}