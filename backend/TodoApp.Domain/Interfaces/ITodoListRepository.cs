using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Repository Interface cho TodoList entity
/// </summary>
public interface ITodoListRepository : IRepository<TodoList>
{
    Task<IReadOnlyList<TodoList>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TodoList?> GetWithItemsAsync(Guid todoListId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoList>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoList>> GetArchivedByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetMaxPositionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

