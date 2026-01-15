using TodoApp.Domain.Content.Entities;

namespace TodoApp.Domain.Content.Interfaces;

// Repository Interface cho Todo
public interface ITodoRepository : IRepository<Todo>
{
    Task<Todo?> GetWithContentItemAsync(Guid todoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetByStatusAsync(Guid workspaceId, TodoStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetByAssigneeAsync(Guid assigneeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetOverdueAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetDueTodayAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Todo>> GetDueThisWeekAsync(Guid workspaceId, CancellationToken cancellationToken = default);
}
