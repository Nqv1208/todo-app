using TodoApp.Domain.Common;

namespace TodoApp.Domain.Events;

/// <summary>
/// Domain Event được raise khi một TodoItem được đánh dấu hoàn thành
/// </summary>
public class TodoItemCompletedEvent : DomainEvent
{
    public Guid TodoItemId { get; }
    public string Title { get; }
    public Guid TodoListId { get; }

    public TodoItemCompletedEvent(Guid todoItemId, string title, Guid todoListId)
    {
        TodoItemId = todoItemId;
        Title = title;
        TodoListId = todoListId;
    }
}

