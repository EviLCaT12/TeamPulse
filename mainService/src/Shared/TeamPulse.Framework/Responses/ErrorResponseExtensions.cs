using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Framework.Responses;

public static class ErrorResponseExtensions
{
    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (errors.Any() == false)
        {
            return new ObjectResult("Have no errors")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        
        var distinctErrorType = errors
            .Select(x => x.Type)
            .Distinct()
            .ToList();
        
        var statusCode = distinctErrorType.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForError(distinctErrorType.First());

        var envelope = Envelope.Error(errors);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };

    }

    private static int GetStatusCodeForError(Error.ErrorType errorType)
    {
        return errorType switch
        {
            Error.ErrorType.Validation => StatusCodes.Status400BadRequest,
            Error.ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}