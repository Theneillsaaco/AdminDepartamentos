﻿namespace AdminDepartamentos.Domain.Exceptions
{
    public class PagoException : Exception
    {
        public PagoException(string message) : base(message)
        {
            // x logica para guardar el error
        }
    }
}
