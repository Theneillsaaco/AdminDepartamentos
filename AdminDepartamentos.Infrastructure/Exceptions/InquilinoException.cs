namespace AdminDepartamentos.Infrastucture.Exceptions;

public class InquilinoException : Exception
{
    public InquilinoException(string message) : base(message)
    {
    }

    public InquilinoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}