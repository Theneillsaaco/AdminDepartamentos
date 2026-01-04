using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.Infrastucture.Interfaces;

public interface IEmailService
{
    void SendEmail(EmailDTO request);
}