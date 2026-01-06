using System.Text;
using AdminDepartamentos.API.Extentions;
using AdminDepartamentos.API.Models;
using AdminDepartamentos.Infrastructure.Context;
using AdminDepartamentos.Infrastructure.Context.Entities;
using AdminDepartamentos.Infrastructure.Interfaces;
using AdminDepartamentos.Infrastructure.Models.ServiceModels;
using Microsoft.Extensions.Options;

namespace AdminDepartamentos.API.Services.BackgroundServices;

public class EmailServices : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (_logger.BeginScope("EmailJobCycle {CycleId}", Guid.NewGuid()))
            {
                try
                {
                    _logger.LogInformation("Email job cycle started.");
                    await ProcessDelayedPaymentsAsync(stoppingToken);
                    _logger.LogInformation("Email job cycle finished successfully.");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Email service cancellation requested.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unhandled error in EmailService execution loop. Errors: {ex.Message}",
                        ex.Message);
                }
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }

        _logger.LogInformation("Email service stopped.");
    }

    private async Task ProcessDelayedPaymentsAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var pagoRepository = scope.ServiceProvider.GetRequiredService<IPagoRepository>();

        var pagos = await pagoRepository.GetRetrasosWithoutEmail();

        _logger.LogInformation("Found {Count} delayed payments without email.", pagos.Count());
        if (!pagos.Any())
            return;

        var retrasoHtml = await GenerateDelayedPaymentsHtml(pagos, pagoRepository, scope, stoppingToken);

        if (string.IsNullOrEmpty(retrasoHtml))
        {
            _logger.LogWarning("No email content generated. Email will not be sent.");
            return;
        }

        var emailContent = await LoadEmailTemplate(retrasoHtml, stoppingToken);
        await SendEmail(emailSender, emailContent);
    }

    private async Task<string> GenerateDelayedPaymentsHtml(IEnumerable<PagoEntity> pagos, IPagoRepository pagoRepository,
        IServiceScope scope, CancellationToken stoppingToken)
    {
        var retrasoHtml = new StringBuilder();
        var dbContext = scope.ServiceProvider.GetRequiredService<DepartContext>();

        foreach (var pago in pagos)
            using (_logger.BeginScope("PagoId {IdPago}", pago.IdPago))
            {
                try
                {
                    _logger.LogInformation("Processing delayed payment for tenant {Tenant}.",
                        $"{pago.Inquilino.FirstName} {pago.Inquilino.LastName}"
                    );

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

                    _logger.LogInformation("Pago marked as emailed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing delayed payment.");
                }
            }

        return retrasoHtml.ToString();
    }

    private async Task<string> LoadEmailTemplate(string retrasoHtml, CancellationToken stoppingToken)
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Emails", "EmailTemplate.html");

        _logger.LogDebug("Loading email template from {Path}", templatePath);

        if (!File.Exists(templatePath))
        {
            _logger.LogError("Email template file not found at {Path}", templatePath);
            throw new FileNotFoundException($"No se encontró la plantilla de correo: {templatePath}");
        }

        using var reader = new StreamReader(templatePath);
        var emailTemplate = await reader.ReadToEndAsync(stoppingToken);

        return emailTemplate.Replace("<!-- Aquí se insertará la lista de inquilinos con pagos retrasados -->",
            retrasoHtml);
    }

    private async Task SendEmail(IEmailService emailSender, string emailContent)
    {
        var emailDto = new EmailDTO
        {
            Para = _emailSettings.EmailSender,
            Asunto = "Pagos Retrasados (No Responder)",
            Contenido = emailContent
        };

        try
        {
            _logger.LogInformation("Sending delayed payments email to {Recipient}.", emailDto.Para);

            emailSender.SendEmail(emailDto);

            _logger.LogInformation("Delayed payments email sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending delayed payments email.");
            throw;
        }
    }

    #region Fields

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailServices> _logger;
    private readonly EmailSettings _emailSettings;

    public EmailServices(IServiceScopeFactory scopeFactory, IOptions<EmailSettings> emailSettings,
        ILogger<EmailServices> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }

    #endregion
}