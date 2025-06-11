using AuthService.Domain;
using AuthService.Errors;
using CSharpFunctionalExtensions;

namespace AuthService.Jwt;

public interface ITokenGenerator
{
    Result<string, Error> GenerateToken(User user);
}