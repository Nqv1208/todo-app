using TodoApp.Domain.Common.Interfaces;
using TodoApp.Domain.Workspace.Entities;

namespace TodoApp.Domain.Workspace.Interfaces;

/// <summary>
/// Repository Interface cho Workspace
/// </summary>
public interface IWorkspaceRepository : IRepository<Entities.Workspace>
{
    Task<Entities.Workspace?> GetWithMembersAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Workspace>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entities.Workspace>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<bool> IsMemberAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}
