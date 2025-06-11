using AuthService.Errors;
using FluentValidation.Results;

namespace AuthService.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;
        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.ErrorMessage, error.ErrorCode, error.InvalidField);
        
        return new ErrorList(errors);
    }
}