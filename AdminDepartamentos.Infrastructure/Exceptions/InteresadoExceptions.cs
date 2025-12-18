namespace AdminDepartamentos.Infrastructure.Exceptions;

public class InteresadoExceptions : Exception
{
    public InteresadoExceptions(string message) : base(message) { }

    public InteresadoExceptions(string message, Exception innerException) : base(message, innerException) { }
}