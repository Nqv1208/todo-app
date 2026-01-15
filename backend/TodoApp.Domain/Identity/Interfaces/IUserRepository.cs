using TodoApp.Domain.Identity.Entities;

namespace TodoApp.Domain.Identity.Interfaces;

// Repository Interface cho User
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetWithSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
