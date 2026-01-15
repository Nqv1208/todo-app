using TodoApp.Domain.Identity.Entities;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Collaboration.Entities;
using TodoApp.Domain.Activity.Entities;
using WorkspaceEntity = TodoApp.Domain.Workspace.Entities.Workspace;
using TodoApp.Domain.Workspace.Entities;

namespace TodoApp.Domain.Common.Interfaces;

/// <summary>
/// Unit of Work Interface - Quản lý transaction và đảm bảo data consistency
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Identity
    IRepository<User> Users { get; }
    IRepository<Session> Sessions { get; }
    
    // Workspace
    IRepository<WorkspaceEntity> Workspaces { get; }
    IRepository<WorkspaceMember> WorkspaceMembers { get; }
    
    // Content
    IRepository<ContentItem> ContentItems { get; }
    IRepository<Block> Blocks { get; }
    IRepository<Todo> Todos { get; }
    
    // Collaboration
    IRepository<Permission> Permissions { get; }
    IRepository<Comment> Comments { get; }
    
    // Activity
    IRepository<ActivityLog> ActivityLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
