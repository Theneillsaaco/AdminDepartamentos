namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoUpdateModel
{
    public int IdInquilino { get; set; }

    public string InquilinoFirstName { get; set; }
    
    public int? NumDeposito { get; set; }

    public decimal Monto { get; set; }
    
    public DateOnly? FechaPago { get; set; } 
}