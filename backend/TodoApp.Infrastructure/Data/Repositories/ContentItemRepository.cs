using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Content.Enums;
using TodoApp.Domain.Content.Interfaces;

namespace TodoApp.Infrastructure.Data.Repositories;

public class ContentItemRepository : Repository<ContentItem>, IContentItemRepository
{
    public ContentItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ContentItem?> GetWithBlocksAsync(Guid contentItemId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Blocks.OrderBy(b => b.Position))
            .FirstOrDefaultAsync(c => c.Id == contentItemId, cancellationToken);
    }

    public async Task<ContentItem?> GetWithChildrenAsync(Guid contentItemId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Children.OrderBy(ch => ch.Position))
            .FirstOrDefaultAsync(c => c.Id == contentItemId, cancellationToken);
    }

    public async Task<ContentItem?> GetWithTodoAsync(Guid contentItemId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Todo)
            .FirstOrDefaultAsync(c => c.Id == contentItemId, cancellationToken);
    }

    public async Task<IReadOnlyList<ContentItem>> GetByWorkspaceIdAsync(
        Guid workspaceId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.WorkspaceId == workspaceId)
            .OrderBy(c => c.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContentItem>> GetByWorkspaceAndTypeAsync(
        Guid workspaceId, 
        ContentType type, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.WorkspaceId == workspaceId && c.Type == type)
            .OrderBy(c => c.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContentItem>> GetByParentIdAsync(
        Guid? parentId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContentItem>> SearchAsync(
        Guid workspaceId, 
        string searchTerm, 
        CancellationToken cancellationToken = default)
    {
        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();

        return await _dbSet
            .Where(c => c.WorkspaceId == workspaceId && 
                       c.Title.ToLower().Contains(normalizedSearchTerm))
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetMaxPositionAsync(
        Guid workspaceId, 
        Guid? parentId, 
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(c => c.WorkspaceId == workspaceId && c.ParentId == parentId);
        
        if (!await query.AnyAsync(cancellationToken))
            return 0;

        return await query.MaxAsync(c => c.Position, cancellationToken);
    }
}
