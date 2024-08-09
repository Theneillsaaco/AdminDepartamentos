using AdminDepartamentos.Domain.Interfaces;
using AdminDepartamentos.Domain.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;

namespace AdminDepartamentos.API.Services.Emails;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }
    
    public void SendEmail(EmailDTO request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
        email.To.Add(MailboxAddress.Parse(request.Para));
        email.Subject = request.Asunto;

        var builder = new BodyBuilder();
        builder.HtmlBody = request.Contenido;

        // Construir las rutas de las imagenes
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Emails");
        
        var logoImagePath = Path.Combine(imagePath, "BlazorStrap.png");
        var checkImagePath = Path.Combine(imagePath, "grey_check.png");
        
        // Imagenes y asignar CID
        var logoImage = builder.LinkedResources.Add(logoImagePath);
        logoImage.ContentId = MimeUtils.GenerateMessageId();

        var checkImage = builder.LinkedResources.Add(checkImagePath);
        checkImage.ContentId = MimeUtils.GenerateMessageId();
        
        // Reemplazar los CIDs en HTML con los reales
        builder.HtmlBody = builder.HtmlBody.Replace("cid:BlazorStrap.png", $"cid:{logoImage.ContentId}")
                                           .Replace("cid:grey_check.png", $"cid:{checkImage.ContentId}");

        email.Body = builder.ToMessageBody();
        
        using var smtp = new SmtpClient();
        smtp.Connect(
            _config.GetSection("Email:Host").Value,
            Convert.ToInt16(_config.GetSection("Email:Port").Value),
            SecureSocketOptions.StartTls
            );

        smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);

        smtp.Send(email);
        smtp.Disconnect(true);
    }
}