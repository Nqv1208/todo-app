using TodoApp.Domain.Common;
using TodoApp.Domain.Enums;
using TodoApp.Domain.Events;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Entity đại diện cho một task/công việc cần làm
/// </summary>
public class TodoItem : AuditableEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TodoStatus Status { get; private set; }
    public TodoPriority Priority { get; private set; }
    public DueDate DueDate { get; private set; } = null!;
    public DateTime? CompletedAt { get; private set; }
    public int Position { get; private set; }
    public string? Notes { get; private set; }

    // Foreign keys
    public Guid TodoListId { get; private set; }
    public Guid? ParentId { get; private set; }

    // Navigation properties
    public TodoList TodoList { get; private set; } = null!;
    public TodoItem? Parent { get; private set; }

    private readonly List<TodoItem> _subTasks = new();
    public IReadOnlyCollection<TodoItem> SubTasks => _subTasks.AsReadOnly();

    private readonly List<Label> _labels = new();
    public IReadOnlyCollection<Label> Labels => _labels.AsReadOnly();

    private TodoItem() : base() { }

    public static TodoItem Create(
        string title, 
        Guid todoListId, 
        string? description = null,
        TodoPriority priority = TodoPriority.None,
        DateTime? dueDate = null,
        Guid? parentId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề không được để trống", nameof(title));

        var todoItem = new TodoItem
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            Status = TodoStatus.Pending,
            Priority = priority,
            DueDate = DueDate.Create(dueDate),
            TodoListId = todoListId,
            ParentId = parentId,
            Position = 0
        };

        // Raise domain event
        todoItem.AddDomainEvent(new TodoItemCreatedEvent(todoItem.Id, todoItem.Title, todoListId));

        return todoItem;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề không được để trống", nameof(title));

        Title = title.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void UpdatePriority(TodoPriority priority)
    {
        Priority = priority;
    }

    public void UpdateDueDate(DateTime? dueDate)
    {
        DueDate = DueDate.Create(dueDate);
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
    }

    public void UpdatePosition(int position)
    {
        if (position < 0)
            throw new ArgumentException("Vị trí không được âm", nameof(position));

        Position = position;
    }

    public void MarkAsCompleted()
    {
        if (Status == TodoStatus.Completed)
            return;

        var previousStatus = Status;
        Status = TodoStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        AddDomainEvent(new TodoItemCompletedEvent(Id, Title, TodoListId));
    }

    public void MarkAsInProgress()
    {
        if (Status == TodoStatus.Completed)
        {
            CompletedAt = null;
        }
        
        Status = TodoStatus.InProgress;
    }

    public void MarkAsPending()
    {
        if (Status == TodoStatus.Completed)
        {
            CompletedAt = null;
        }
        
        Status = TodoStatus.Pending;
    }

    public void Cancel()
    {
        Status = TodoStatus.Cancelled;
    }

    public void PutOnHold()
    {
        Status = TodoStatus.OnHold;
    }

    public void MoveToList(Guid newTodoListId)
    {
        TodoListId = newTodoListId;
    }

    public void AddLabel(Label label)
    {
        if (!_labels.Contains(label))
        {
            _labels.Add(label);
        }
    }

    public void RemoveLabel(Label label)
    {
        _labels.Remove(label);
    }

    public void ClearLabels()
    {
        _labels.Clear();
    }

    public void AddSubTask(TodoItem subTask)
    {
        _subTasks.Add(subTask);
    }

    public void RemoveSubTask(TodoItem subTask)
    {
        _subTasks.Remove(subTask);
    }

    public bool IsOverdue => DueDate.IsOverdue && Status != TodoStatus.Completed;
    
    public bool HasSubTasks => _subTasks.Count > 0;
    
    public int CompletedSubTasksCount => _subTasks.Count(st => st.Status == TodoStatus.Completed);
    
    public double SubTasksCompletionPercentage => HasSubTasks 
        ? (double)CompletedSubTasksCount / _subTasks.Count * 100 
        : 0;
}

