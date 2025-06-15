using TeamPulse.Accounts.Domain.Models;

namespace TeamPulse.Accounts.Application;

public interface ITokenProvider
{
    string GenerateTokenAsync(User user, CancellationToken cancellationToken);
}