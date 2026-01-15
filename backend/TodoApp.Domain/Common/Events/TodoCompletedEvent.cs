namespace TodoApp.Domain.Common.Events;

// Domain Event khi Todo được hoàn thành
public class TodoCompletedEvent : BaseEvent
{
    public Guid TodoId { get; }
    public Guid ContentItemId { get; }
    public string Title { get; }
    public Guid WorkspaceId { get; }
    public Guid CompletedByUserId { get; }

    public TodoCompletedEvent(Guid todoId, Guid contentItemId, string title, Guid workspaceId, Guid completedByUserId)
    {
        TodoId = todoId;
        ContentItemId = contentItemId;
        Title = title;
        WorkspaceId = workspaceId;
        CompletedByUserId = completedByUserId;
    }
}
