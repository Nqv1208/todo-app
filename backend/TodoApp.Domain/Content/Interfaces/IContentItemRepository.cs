using TodoApp.Domain.Content.Entities;

namespace TodoApp.Domain.Content.Interfaces;

// Repository Interface cho ContentItem
public interface IContentItemRepository : IRepository<ContentItem>
{
    Task<ContentItem?> GetWithBlocksAsync(Guid contentItemId, CancellationToken cancellationToken = default);
    Task<ContentItem?> GetWithChildrenAsync(Guid contentItemId, CancellationToken cancellationToken = default);
    Task<ContentItem?> GetWithTodoAsync(Guid contentItemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentItem>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentItem>> GetByWorkspaceAndTypeAsync(Guid workspaceId, ContentType type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentItem>> GetByParentIdAsync(Guid? parentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContentItem>> SearchAsync(Guid workspaceId, string searchTerm, CancellationToken cancellationToken = default);
    Task<int> GetMaxPositionAsync(Guid workspaceId, Guid? parentId, CancellationToken cancellationToken = default);
}
