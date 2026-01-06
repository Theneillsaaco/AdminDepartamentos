namespace AdminDepartamentos.Infrastructure.Models.PagoModels;

public class PagoWithoutEmail
{
    public int IdPago { get; set; }

    public int IdInquilino { get; set; }

    public string InquilinoFirstName { get; set; }

    public string InquilinoLastName { get; set; }

    public bool Retrasado { get; set; }

    public bool Deleted { get; set; }

    public bool Email { get; set; }
}