using System.Data;

namespace TeamPulse.Core.Abstractions;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public enum ModuleKey
{
    Team,
    Performance
}