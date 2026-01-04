using AdminDepartamentos.Domain.Models;
using AdminDepartamentos.Infrastucture.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;

namespace AdminDepartamentos.API.Services.Emails;

public class EmailService : IEmailService
{
    public void SendEmail(EmailDTO request)
    {
        // Crear el mensaje del correo
        var email = CreateEmailMessage(request);

        // Enviar el correo
        using var smtp = new SmtpClient();
        smtp.Connect(
            _config["Email:Host"],
            int.Parse(_config["Email:Port"]),
            SecureSocketOptions.StartTls);

        smtp.Authenticate(_config["Email:UserName"], _config["Email:Password"]);
        smtp.Send(email);
        smtp.Disconnect(true);
    }

    private MimeMessage CreateEmailMessage(EmailDTO request)
    {
        var email = new MimeMessage
        {
            Subject = request.Asunto
        };
        email.From.Add(MailboxAddress.Parse(_config["Email:UserName"]));
        email.To.Add(MailboxAddress.Parse(request.Para));

        var builder = new BodyBuilder
        {
            HtmlBody = request.Contenido
        };

        AddLinkedResources(builder);

        email.Body = builder.ToMessageBody();

        return email;
    }

    private void AddLinkedResources(BodyBuilder builder)
    {
        var imagePaths = new Dictionary<string, string>
        {
            { "BlazorStrap.png", "cid:BlazorStrap.png" },
            { "grey_check_v2.png", "cid:grey_check_v2.png" }
        };

        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Emails");

        foreach (var (fileName, placeholder) in imagePaths)
        {
            var filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath)) continue;

            var linkedResource = builder.LinkedResources.Add(filePath);
            linkedResource.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = builder.HtmlBody.Replace(placeholder, $"cid:{linkedResource.ContentId}");
        }
    }

    #region Fields

    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    #endregion
}