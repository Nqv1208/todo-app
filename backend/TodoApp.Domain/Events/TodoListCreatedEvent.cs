using TodoApp.Domain.Common;

namespace TodoApp.Domain.Events;

/// <summary>
/// Domain Event được raise khi một TodoList mới được tạo
/// </summary>
public class TodoListCreatedEvent : DomainEvent
{
    public Guid TodoListId { get; }
    public string Name { get; }
    public Guid UserId { get; }

    public TodoListCreatedEvent(Guid todoListId, string name, Guid userId)
    {
        TodoListId = todoListId;
        Name = name;
        UserId = userId;
    }
}

