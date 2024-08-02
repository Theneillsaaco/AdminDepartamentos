using AdminDepartamentos.API.Models.PagoModels.Core;

namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoGetByInquilinoModel : PagoInquilinoBaseModel
{
    public string InquilinoFirstName { get; set; }

    public string InquilinoLastName { get; set; }
}