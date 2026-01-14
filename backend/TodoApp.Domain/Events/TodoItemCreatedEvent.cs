using TodoApp.Domain.Common;

namespace TodoApp.Domain.Events;

/// <summary>
/// Domain Event được raise khi một TodoItem mới được tạo
/// </summary>
public class TodoItemCreatedEvent : DomainEvent
{
    public Guid TodoItemId { get; }
    public string Title { get; }
    public Guid TodoListId { get; }

    public TodoItemCreatedEvent(Guid todoItemId, string title, Guid todoListId)
    {
        TodoItemId = todoItemId;
        Title = title;
        TodoListId = todoListId;
    }
}

