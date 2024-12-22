namespace AdminDepartamentos.App.Models.PagoModels;

public class PagoGetModel
{
    public int IdPago { get; set; }

    public string InquilinoFirstName { get; set; }
    
    public string InquilinoLastName { get; set; }
    
    public int? NumDeposito { get; set; }

    public int FechaPagoInDays { get; set; }
    
    public bool Retrasado { get; set; }
}