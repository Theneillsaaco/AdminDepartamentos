using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Domain.Interfaces;

public interface IEmailService
{
    void SendEmail(EmailDTO request);
}