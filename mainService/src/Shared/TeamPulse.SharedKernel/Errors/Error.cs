namespace TeamPulse.SharedKernel.Errors;

public class Error
{
    private Error(string message, string code, ErrorType errorType, string? invalidField = null)
    {
        ErrorMessage = message;
        ErrorCode = code;
        Type = errorType;
        InvalidField = invalidField;
    }

    public const string SEPARATOR = "||";
    public string ErrorCode { get; init; } 
    
    public string ErrorMessage { get; init; }
    
    public ErrorType Type { get; init; }
    
    public string? InvalidField { get; init; }

    public static Error Validation(string message, string code, string? invalidField = null) =>
        new(message, code, ErrorType.Validation, invalidField);
    
    public static Error NotFound(string message, string code) =>
        new(message, code, ErrorType.NotFound);
    
    public static Error Failure(string message, string code) =>
        new(message, code, ErrorType.Failure);

    public string Serialize()
    {
        return string.Join(SEPARATOR, ErrorMessage, ErrorCode, Type, InvalidField);
    }

    public static Error Deserialize(string errorString)
    {
        var parts = errorString.Split(SEPARATOR);
        if (parts.Length < 3)
            throw new ArgumentException($"Invalid error string {errorString}");
        
        if (Enum.TryParse<ErrorType>(parts[2], out var errorType) == false)
            throw new ArgumentException($"Invalid error type {errorString}");

        return new Error(parts[0], parts[1], errorType);
    }

    public ErrorList ToErrorList()
    {
        return new ErrorList([this]);
    }
    
    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure,
        Conflict
    }
    
}