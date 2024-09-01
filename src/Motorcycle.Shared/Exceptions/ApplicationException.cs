namespace Motorcycle.Shared.Exceptions;

public class ApplicationException : Exception
{
    protected ApplicationException(string message) : base(message)
    {
    }
}