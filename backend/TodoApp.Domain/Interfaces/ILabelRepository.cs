using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Repository Interface cho Label entity
/// </summary>
public interface ILabelRepository : IRepository<Label>
{
    Task<IReadOnlyList<Label>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Label?> GetWithTodoItemsAsync(Guid labelId, CancellationToken cancellationToken = default);
    Task<bool> IsNameExistsAsync(Guid userId, string name, CancellationToken cancellationToken = default);
}

