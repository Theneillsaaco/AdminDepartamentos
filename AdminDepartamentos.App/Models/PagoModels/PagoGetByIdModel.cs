namespace AdminDepartamentos.App.Models;

public class PagoGetByIdModel
{
    public int IdPago { get; set; }
    
    public int? NumDeposito { get; set; }

    public decimal Monto { get; set; }
    
    public int FechaPagoInDays { get; set; }
    
    public bool Email { get; set; }
    
    public bool Retrasado { get; set; }
    
    public bool Deleted { get; set; }
}