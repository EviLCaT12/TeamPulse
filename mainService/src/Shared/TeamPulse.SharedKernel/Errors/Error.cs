namespace TeamPulse.SharedKernel.Errors;

public class Error
{
    private Error(string message, string code, ErrorType errorType)
    {
        ErrorMessage = message;
        ErrorCode = code;
        Type = errorType;
    }
    
    public string ErrorCode { get; init; } 
    
    public string ErrorMessage { get; init; }
    
    public ErrorType Type { get; init; }

    public static Error Validation(string message, string code) =>
        new(message, code, ErrorType.Validation);
    
    public static Error NotFound(string message, string code) =>
        new(message, code, ErrorType.NotFound);
    
    public static Error Failure(string message, string code) =>
        new(message, code, ErrorType.Failure);
    
    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure
    }
    
}