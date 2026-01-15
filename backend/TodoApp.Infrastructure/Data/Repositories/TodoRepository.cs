using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Content.Enums;
using TodoApp.Domain.Content.Interfaces;

namespace TodoApp.Infrastructure.Data.Repositories;

public class TodoRepository : Repository<Todo>, ITodoRepository
{
    public TodoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Todo?> GetWithContentItemAsync(Guid todoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.ContentItem)
            .FirstOrDefaultAsync(t => t.Id == todoId, cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetByWorkspaceIdAsync(
        Guid workspaceId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.ContentItem.WorkspaceId == workspaceId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetByStatusAsync(
        Guid workspaceId, 
        TodoStatus status, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.ContentItem.WorkspaceId == workspaceId && t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetByAssigneeAsync(
        Guid assigneeId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.AssigneeId == assigneeId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetOverdueAsync(
        Guid workspaceId, 
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.ContentItem.WorkspaceId == workspaceId &&
                       t.Status != TodoStatus.Done &&
                       t.DueDate.Value != null &&
                       t.DueDate.Value < now)
            .OrderBy(t => t.DueDate.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetDueTodayAsync(
        Guid workspaceId, 
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.ContentItem.WorkspaceId == workspaceId &&
                       t.Status != TodoStatus.Done &&
                       t.DueDate.Value != null &&
                       t.DueDate.Value >= today &&
                       t.DueDate.Value < tomorrow)
            .OrderBy(t => t.DueDate.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Todo>> GetDueThisWeekAsync(
        Guid workspaceId, 
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek + 1);

        return await _dbSet
            .Include(t => t.ContentItem)
            .Where(t => t.ContentItem.WorkspaceId == workspaceId &&
                       t.Status != TodoStatus.Done &&
                       t.DueDate.Value != null &&
                       t.DueDate.Value >= today &&
                       t.DueDate.Value < endOfWeek)
            .OrderBy(t => t.DueDate.Value)
            .ToListAsync(cancellationToken);
    }
}
