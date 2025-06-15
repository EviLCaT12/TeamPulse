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
    }
}