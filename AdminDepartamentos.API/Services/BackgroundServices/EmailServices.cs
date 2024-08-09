using System.Text;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using Microsoft.Extensions.Options;

namespace AdminDepartamentos.API.Services.BackgroundServices;

public class EmailServices : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmailSettings _emailSettings;

    public EmailServices(IServiceScopeFactory scopeFactory, IOptions<EmailSettings> emailSettings)
    {
        _scopeFactory = scopeFactory;
        _emailSettings = emailSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();
                var pagos = await pagoRepository.GetRetrasosWithoutEmail();

                List<Pago> pagosUpdate = [];
                
                if (pagos.Any())
                {
                    var inquilinosConRetraso = new StringBuilder();

                    foreach (var pago in pagos)
                    {
                        try
                        {
                            pago.ConvertPagoEntityToPagoWithoutEmail();
                            
                            inquilinosConRetraso.Append("<div style='border-bottom: 1px solid #ccc; padding-bottom: 10px; margin-bottom: 10px;'>")
                                                .Append($"<p>Inquilino: {pago.Inquilino.FirstName} {pago.Inquilino.LastName} - Pago Id: {pago.IdPago}</p>")
                                                .Append("</div>");

                            pago.Email = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error checking retraso for pago: {ex.Message}");
                            continue;
                        }
                        
                        pagosUpdate.Add(pago);
                    }

                    string emailTemplate;
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Emails", "EmailBody.html");
                    using (var reader = new StreamReader(templatePath))
                    {
                        emailTemplate = await reader.ReadToEndAsync(stoppingToken);
                    }

                    string emailContent = emailTemplate.Replace("<!-- Aquí se insertará la lista de inquilinos con pagos retrasados -->", inquilinosConRetraso.ToString());

                    var emailDto = new EmailDTO
                    {
                        Para = _emailSettings.EmailSender,
                        Asunto = "Pagos Retrasados (No Responder)",
                        Contenido = emailContent
                    };

                    try
                    {
                        emailSender.SendEmail(emailDto);
                        
                        await pagoRepository.Update(pagosUpdate);
                        await scope.ServiceProvider.GetRequiredService<DepartContext>().SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending email or updating pagos: {ex.Message}");
                        throw;
                    }
                }
            }

            Console.WriteLine("Ejecutando EmailService");

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}