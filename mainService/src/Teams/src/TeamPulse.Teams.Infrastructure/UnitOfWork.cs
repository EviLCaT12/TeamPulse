using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _context;

    public UnitOfWork(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}