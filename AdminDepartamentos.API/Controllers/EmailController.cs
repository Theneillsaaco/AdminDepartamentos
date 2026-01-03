using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDepartamentos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmailController : ControllerBase
{
    [HttpPost]
    public IActionResult SendEmail(EmailDTO request)
    {
        _logger.LogInformation("Send Email - Start.");

        try
        {
            _emailSender.SendEmail(request);
            _logger.LogInformation("Send Email - Email sent.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Send Email - Error. Request: {request}", request);
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }

        return Ok();
    }

    #region Fields

    private readonly IEmailService _emailSender;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IEmailService emailSender, ILogger<EmailController> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    #endregion
}