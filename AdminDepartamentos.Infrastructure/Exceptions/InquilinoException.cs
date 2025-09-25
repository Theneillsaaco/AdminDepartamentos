namespace AdminDepartamentos.Infrastructure.Exceptions;

public class InquilinoException : Exception
{
    public InquilinoException(string message) : base(message)
    {
        LogError(message, null);
    }

    public InquilinoException(string message, Exception innerException) : base(message, innerException)
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
                              $"InquilinoException: {message}" +
                              $"{(inner != null ? $" | Inner: {inner.Message}" : "")}{Environment.NewLine}";
            File.AppendAllText(logPath, errorMsg);
        }
        catch { }
    }
}