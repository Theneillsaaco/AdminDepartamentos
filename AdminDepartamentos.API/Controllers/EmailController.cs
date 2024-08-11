using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminDepartamentos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailSender;

    public EmailController(IEmailService emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpPost]
    public IActionResult SendEmail(EmailDTO request)
    {
        try
        {
            _emailSender.SendEmail(request);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.InnerException?.Message ?? ex.Message);
        }
        
        return Ok();
    }
}