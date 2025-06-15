using TeamPulse.Accounts.Domain.Models;

namespace TeamPulse.Accounts.Application;

public interface ITokenProvider
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken);
}