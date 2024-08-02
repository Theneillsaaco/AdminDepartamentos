namespace AdminDepartamentos.API.Models.PagoModels.Core;

public abstract class PagoViewBaseModel
{
    public int? NumDeposito { get; set; }

    public int? FechaPagoInDays { get; set; }
}