namespace AdminDepartamentos.Infrastructure.Exceptions;

public class UnidadHabitacionalException : Exception
{
    public UnidadHabitacionalException(string message) : base(message) { }

    public UnidadHabitacionalException(string message, Exception? innerException) : base(message, innerException) { }
}