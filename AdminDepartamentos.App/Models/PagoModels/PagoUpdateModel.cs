namespace AdminDepartamentos.App.Models;

public class PagoUpdateModel
{
    public int IdPago { get; set; }

    public int IdInquilino { get; set; }

    public bool Retrasado { get; set; }
    
    public int? NumDeposito { get; set; }
    
    public decimal Monto { get; set; }
    
    public int FechaPagoInDays { get; set; }
}