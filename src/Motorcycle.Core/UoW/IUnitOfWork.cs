namespace Motorcycle.Core.UoW;

public interface IUnitOfWork
{
    bool Commit();

    Task<bool> CommitAsync(CancellationToken ct);

    void BeginTransaction();

    void CommitTransaction();

    void RollbackTransaction();

    void DisposeTransaction();
}