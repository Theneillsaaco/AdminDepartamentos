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
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Console.WriteLine("Email service running.");
                await ProcessDelayedPaymentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EmailService: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
    
    private async Task ProcessDelayedPaymentsAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();
        var pagos = await pagoRepository.GetRetrasosWithoutEmail();

        if (!pagos.Any())
            return;
        
        var retrasoHtml = await GenerateDelayedPaymentsHtml(pagos, pagoRepository, scope, stoppingToken);

        if (!string.IsNullOrEmpty(retrasoHtml))
        {
            string emailContent = await LoadEmailTemplate(retrasoHtml, stoppingToken);
            await SendEmail(emailSender, emailContent);
        }
    }

    private async Task<string> GenerateDelayedPaymentsHtml(IEnumerable<Pago> pagos, IPagoRepository pagoRepository,
        IServiceScope scope, CancellationToken stoppingToken)
    {
        var retrasoHtml = new StringBuilder();
        var dbContext = scope.ServiceProvider.GetRequiredService<DepartContext>();

        foreach (var pago in pagos)
        {
            try
            {
                pago.ConvertPagoEntityToPagoWithoutEmail();
                pago.Email = true;
                
                await pagoRepository.Update(pago);
                await dbContext.SaveChangesAsync(stoppingToken);
                
                retrasoHtml.Append($@"
                     <div class='tenant-item'>
                        <strong>Inquilino:</strong> {pago.Inquilino.FirstName} {pago.Inquilino.LastName} <br>
                        <strong>Pago Id:</strong> {pago.IdPago}
                    </div>
                ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing pago (Id: {pago.IdPago}): {ex.Message}");
            }
        }
        
        return retrasoHtml.ToString();
    }

    private async Task<string> LoadEmailTemplate(string retrasoHtml, CancellationToken stoppingToken)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Emails", "EmailTemplate.html");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"No se encontró la plantilla de correo: {templatePath}");

        using var reader = new StreamReader(templatePath);
        var emailTemplate = await reader.ReadToEndAsync(stoppingToken);
        
        return emailTemplate.Replace("<!-- Aquí se insertará la lista de inquilinos con pagos retrasados -->", retrasoHtml);
    }

    private async Task SendEmail(IEmailService emailSender, string emailContent)
    {
        var emailDT0 = new EmailDTO
        {
            Para = _emailSettings.EmailSender,
            Asunto = "Pagos Retrasados (No Responder)",
            Contenido = emailContent
        };

        try
        {
            emailSender.SendEmail(emailDT0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando correo: {ex.Message}");
            throw;
        }
    }
    
    #region Fields
    
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmailSettings _emailSettings;

    public EmailServices(IServiceScopeFactory scopeFactory, IOptions<EmailSettings> emailSettings)
    {
        _scopeFactory = scopeFactory;
        _emailSettings = emailSettings.Value;
    }
    
    #endregion
}