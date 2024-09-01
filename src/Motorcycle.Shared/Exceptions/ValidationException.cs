namespace Motorcycle.Shared.Exceptions;

public sealed class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base("One or more validation errors occurred")
    {
        ErrorsDictionary = errorsDictionary;
    }

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}