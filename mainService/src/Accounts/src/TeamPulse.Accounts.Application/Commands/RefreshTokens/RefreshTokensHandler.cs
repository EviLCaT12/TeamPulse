using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Accounts.Application.AccountManagers;
using TeamPulse.Accounts.Contracts.Responses;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework.Authorization;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Application.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<LoginResponse, RefreshTokensCommand>
{
    private readonly IRefreshSessionManager _refreshSessionManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(
        IRefreshSessionManager refreshSessionManager,
        ITokenProvider tokenProvider,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork)
    {
        _refreshSessionManager = refreshSessionManager;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponse, ErrorList>> HandleAsync(RefreshTokensCommand command,
        CancellationToken cancellationToken)
    {
        var getRefreshSessionResult = await _refreshSessionManager
            .GetByRefreshToken(command.RefreshToken, cancellationToken);

        if (getRefreshSessionResult.IsFailure)
            return getRefreshSessionResult.Error.ToErrorList();

        var refreshSession = getRefreshSessionResult.Value;

        if (refreshSession.ExpiresIn < DateTime.UtcNow)
        {
            return Errors.Tokens.TokenIsExpired("Your token has expired").ToErrorList();
        }

        var getUserClaimsValidationResult = await _tokenProvider
            .GetUserClaims(command.AccessToken, cancellationToken);

        if (getUserClaimsValidationResult.IsFailure)
            return getUserClaimsValidationResult.Error;

        var userClaims = getUserClaimsValidationResult.Value;

        var userIdString = userClaims.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Errors.General.Failure("Something went wrong").ToErrorList();
        }

        if (refreshSession.UserId != userId)
            return Errors.Tokens.TokenIsInvalid().ToErrorList();

        var jtiString = userClaims.FirstOrDefault(c => c.Type == CustomClaims.Jti)?.Value;
        if (!Guid.TryParse(jtiString, out var jti))
        {
            return Errors.General.Failure("Something went wrong").ToErrorList();
        }

        if (refreshSession.Jti != jti)
        {
            return Errors.Tokens.TokenIsInvalid().ToErrorList();
        }

        _refreshSessionManager.DeleteRefreshSession(refreshSession);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = await _tokenProvider
            .GenerateAccessTokenAsync(refreshSession.User, cancellationToken);
        
        var refreshToken = await _tokenProvider
            .GenerateRefreshTokenAsync(refreshSession.User, accessToken.Jti, cancellationToken);
        
        return new LoginResponse(accessToken.AccessToken, refreshToken);
    }
}