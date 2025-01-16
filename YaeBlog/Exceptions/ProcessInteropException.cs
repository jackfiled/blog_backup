namespace YaeBlog.Core.Exceptions;

public sealed class ProcessInteropException : Exception
{
    public ProcessInteropException(string message) : base(message)
    {
    }

    public ProcessInteropException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
