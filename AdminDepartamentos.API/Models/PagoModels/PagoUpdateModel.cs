﻿using AdminDepartamentos.API.Models.PagoModels.Core;

namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoUpdateModel : PagoViewBaseModel
{
    public decimal Monto { get; set; }
}