using AdminDepartamentos.Infrastructure.Models.ServiceModels;

namespace AdminDepartamentos.Infrastructure.Interfaces;

public interface IEmailService
{
    void SendEmail(EmailDTO request);
}