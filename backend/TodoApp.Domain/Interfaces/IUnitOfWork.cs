namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Unit of Work Interface - Quản lý transaction và đảm bảo data consistency
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITodoListRepository TodoLists { get; }
    ITodoItemRepository TodoItems { get; }
    ILabelRepository Labels { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

