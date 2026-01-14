using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Repository Interface cho User entity
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetWithTodoListsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetWithLabelsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
}

