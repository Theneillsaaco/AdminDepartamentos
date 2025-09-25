namespace AdminDepartamentos.Infrastructure.Exceptions;

public class InteresadoExceptions : Exception
{
    public InteresadoExceptions(string message) : base(message)
    {
        LogError(message, null);
    }

    public InteresadoExceptions(string message, Exception innerException) : base(message, innerException)
    {
        LogError(message, innerException);
    }
    
    private void LogError(string message, Exception? inner)
    {
        try
        {
            string logPath = Path.Combine(AppContext.BaseDirectory, "logs", "errors.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            
            string errorMsg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]" + 
                              $"InteresadoException: {message}" +
                              $"{(inner != null ? $" | Inner: {inner.Message}" : "")}{Environment.NewLine}";
            File.AppendAllText(logPath, errorMsg);
        }
        catch { }
    }
}