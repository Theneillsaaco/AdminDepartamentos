﻿using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Extentions;

public static class InquilinoViewExtenticion
{
    public static InquilinoViewModel ConvertInquilinoViewModelToInquilinoModel(this InquilinoModel inquilinoModel)
    {
        return new InquilinoViewModel
        {
            IdInquilino = inquilinoModel.IdInquilino,
            FirstName = inquilinoModel.FirstName,
            LastName = inquilinoModel.LastName,
            Cedula = inquilinoModel.Cedula,
            NumTelefono = inquilinoModel.NumTelefono
        };
    }

    public static Inquilino ConvertEntityInquilinoToInquilinoUpdateModel(this InquilinoUpdateModel inquilinoUpdate)
    {
        return new Inquilino
        {
            FirstName = inquilinoUpdate.FirstName,
            LastName = inquilinoUpdate.LastName,
            Cedula = inquilinoUpdate.Cedula,
            Telefono = inquilinoUpdate.NumTelefono,
            ModifyDate = DateTime.Now
        };
    }
}