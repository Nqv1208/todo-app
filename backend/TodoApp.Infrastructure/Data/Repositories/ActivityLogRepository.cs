using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Activity.Entities;
using TodoApp.Domain.Activity.Enums;
using TodoApp.Domain.Activity.Interfaces;

namespace TodoApp.Infrastructure.Data.Repositories;

public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
{
    public ActivityLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByWorkspaceIdAsync(
        Guid workspaceId, 
        int skip = 0, 
        int take = 50, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.WorkspaceId == workspaceId)
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByActorIdAsync(
        Guid actorId, 
        int skip = 0, 
        int take = 50, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.ActorId == actorId)
            .OrderByDescending(a => a.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByTargetAsync(
        string targetType, 
        Guid targetId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.TargetType == targetType && a.TargetId == targetId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetByActionAsync(
        Guid workspaceId, 
        ActivityAction action, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.WorkspaceId == workspaceId && a.Action == action)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityLog>> GetRecentAsync(
        Guid workspaceId, 
        DateTime since, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(a => a.WorkspaceId == workspaceId && a.CreatedAt >= since)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
