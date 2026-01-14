using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Repository Interface cho TodoItem entity
/// </summary>
public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetByTodoListIdAsync(Guid todoListId, CancellationToken cancellationToken = default);
    Task<TodoItem?> GetWithSubTasksAsync(Guid todoItemId, CancellationToken cancellationToken = default);
    Task<TodoItem?> GetWithLabelsAsync(Guid todoItemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetByStatusAsync(Guid todoListId, TodoStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetByPriorityAsync(Guid todoListId, TodoPriority priority, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetOverdueAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetDueTodayAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetDueThisWeekAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> GetByLabelAsync(Guid labelId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoItem>> SearchAsync(Guid userId, string searchTerm, CancellationToken cancellationToken = default);
    Task<int> GetMaxPositionByTodoListIdAsync(Guid todoListId, CancellationToken cancellationToken = default);
}

