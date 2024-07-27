using AdminDepartamentos.API.Models.PagoModels.Core;

namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoViewModel : PagoViewBaseModel
{
    public int IdPago { get; set; }
    
    public int IdInquilino { get; set; }

    public bool Retrasado { get; set; }
}