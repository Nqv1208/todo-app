using TodoApp.Domain.Content.Enums;

namespace TodoApp.Domain.Common.Events;

/// <summary>
/// Domain Event khi ContentItem được tạo
/// </summary>
public class ContentItemCreatedEvent : BaseEvent
{
    public Guid ContentItemId { get; }
    public ContentType Type { get; }
    public string Title { get; }
    public Guid WorkspaceId { get; }
    public Guid CreatedByUserId { get; }

    public ContentItemCreatedEvent(Guid contentItemId, ContentType type, string title, Guid workspaceId, Guid createdByUserId)
    {
        ContentItemId = contentItemId;
        Type = type;
        Title = title;
        WorkspaceId = workspaceId;
        CreatedByUserId = createdByUserId;
    }
}
