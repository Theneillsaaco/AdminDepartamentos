namespace AdminDepartamentos.App.Models.InquilinoModels;

public class PagoDto
{
    public int? NumDeposito { get; set; }
    
    public decimal Monto { get; set; }
    
    public int FechaPagoInDays { get; set; }
}