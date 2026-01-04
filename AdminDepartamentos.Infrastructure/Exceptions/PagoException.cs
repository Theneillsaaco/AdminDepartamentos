namespace AdminDepartamentos.Infrastucture.Exceptions;

public class PagoException : Exception
{
    public PagoException(string message) : base(message)
    {
    }

    public PagoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}