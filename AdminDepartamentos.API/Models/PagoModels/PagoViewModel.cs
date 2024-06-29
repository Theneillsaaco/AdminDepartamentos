namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoViewModel
{
    public int IdPago { get; set; }
    
    public int IdInquilino { get; set; }
    
    public int? NumDeposito { get; set; }
    
    public DateOnly? FechaPago { get; set; }

    public bool Retrasado { get; set; }
}