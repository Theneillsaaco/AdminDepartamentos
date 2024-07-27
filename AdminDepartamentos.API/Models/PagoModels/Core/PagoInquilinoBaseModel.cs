namespace AdminDepartamentos.API.Models.PagoModels.Core;

public abstract class PagoInquilinoBaseModel : PagoViewBaseModel
{
    public int IdPago { get; set; }
    
    public int IdInquilino { get; set; }
    
    public bool Retrasado { get; set; }
}