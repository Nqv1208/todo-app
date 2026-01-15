using TodoApp.Domain.Activity.Entities;

namespace TodoApp.Domain.Activity.Interfaces;

// Repository Interface cho ActivityLog
public interface IActivityLogRepository : IRepository<ActivityLog>
{
    Task<IReadOnlyList<ActivityLog>> GetByWorkspaceIdAsync(
        Guid workspaceId, 
        int skip = 0, 
        int take = 50, 
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<ActivityLog>> GetByActorIdAsync(
        Guid actorId, 
        int skip = 0, 
        int take = 50, 
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<ActivityLog>> GetByTargetAsync(
        string targetType, 
        Guid targetId, 
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<ActivityLog>> GetByActionAsync(
        Guid workspaceId, 
        ActivityAction action, 
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<ActivityLog>> GetRecentAsync(
        Guid workspaceId, 
        DateTime since, 
        CancellationToken cancellationToken = default);
}
