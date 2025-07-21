namespace AdminDepartamentos.Domain.Models;

public class PagoInquilinoModel
{
    public int IdPago { get; set; }

    public int IdInquilino { get; set; }

    public string InquilinoFirstName { get; set; }

    public string InquilinoLastName { get; set; }

    public int? NumDeposito { get; set; }

    public decimal Monto { get; set; }

    public int FechaPagoInDays { get; set; }
    
    public bool Email { get; set; }

    public bool Retrasado { get; set; }
}