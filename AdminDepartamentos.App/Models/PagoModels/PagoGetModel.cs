namespace AdminDepartamentos.App.Models;

public class PagoGetModel
{
    public int IdPago { get; set; }

    public int IdInquilino { get; set; }

    public string InquilinoFirstName { get; set; }
    
    public string InquilinoLastName { get; set; }
    
    public bool Email { get; set; }
    
    public int? NumDeposito { get; set; }

    public int FechaPagoInDays { get; set; }
    
    public bool Retrasado { get; set; }
    
    public bool Deleted { get; set; }
}