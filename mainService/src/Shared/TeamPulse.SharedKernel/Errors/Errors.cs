namespace TeamPulse.SharedKernel.Errors;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsRequired(string errorMessage) =>
            Error.Validation(errorMessage, "value.is.required");
        
        public static Error ValueIsInvalid(string errorMessage) =>
            Error.Validation(errorMessage, "value.is.invalid");
        
        public static Error ValueNotFound(string errorMessage) =>
            Error.NotFound(errorMessage, "value.not.found");
        
        public static Error AlreadyExists(string errorMessage) =>
            Error.Validation(errorMessage, "value.already.exists");
        
        public static Error Failure(string errorMessage) =>
            Error.Failure(errorMessage, "failure");
    }
    
    public static class Tokens
    {
        public static Error TokenIsExpired(string errorMessage) =>
            Error.Validation(errorMessage, "token.is.expired");
        
        public static Error TokenIsInvalid(string? errorMessage = null) =>
            Error.Validation(errorMessage ?? "Your token is invalid", "token.is.invalid");
    }
}