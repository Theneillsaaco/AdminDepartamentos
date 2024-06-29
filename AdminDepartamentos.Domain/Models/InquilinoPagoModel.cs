using AdminDepartamentos.Domain.Entities;

namespace AdminDepartamentos.Domain.Models;

public class InquilinoPagoModel
{
    public Inquilino Inquilino { get; set; }
    public Pago Pago { get; set; }
}