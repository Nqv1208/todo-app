using TodoApp.Domain.Content.ValueObjects;

namespace TodoApp.Domain.Content.Entities;

// Entity đại diện cho Todo
// Todo không đứng một mình - nó luôn liên kết với ContentItem
public class Todo : AuditableEntity
{
    public Guid ContentItemId { get; private set; }
    public TodoStatus Status { get; private set; }
    public TodoPriority Priority { get; private set; }
    public DueDate DueDate { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Subtasks as Value Object collection
    private readonly List<SubTask> _subTasks = new();
    public IReadOnlyCollection<SubTask> SubTasks => _subTasks.AsReadOnly();

    // Navigation
    public ContentItem ContentItem { get; private set; } = null!;

    private Todo() : base() { }

    public static Todo Create(Guid contentItemId, TodoPriority priority = TodoPriority.None, DateTime? dueDate = null)
    {
        return new Todo
        {
            ContentItemId = contentItemId,
            Status = TodoStatus.Todo,
            Priority = priority,
            DueDate = DueDate.Create(dueDate)
        };
    }

    public void UpdatePriority(TodoPriority priority)
    {
        Priority = priority;
    }

    public void UpdateDueDate(DateTime? dueDate)
    {
        DueDate = DueDate.Create(dueDate);
    }

    public void AssignTo(Guid? userId)
    {
        AssigneeId = userId;
    }

    public void MarkAsTodo()
    {
        Status = TodoStatus.Todo;
        CompletedAt = null;
    }

    public void MarkAsDoing()
    {
        Status = TodoStatus.Doing;
        CompletedAt = null;
    }

    public void MarkAsDone()
    {
        if (Status != TodoStatus.Done)
        {
            Status = TodoStatus.Done;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public SubTask AddSubTask(string title)
    {
        var subTask = SubTask.Create(title);
        _subTasks.Add(subTask);
        return subTask;
    }

    public void RemoveSubTask(Guid subTaskId)
    {
        var subTask = _subTasks.FirstOrDefault(st => st.Id == subTaskId);
        if (subTask != null)
        {
            _subTasks.Remove(subTask);
        }
    }

    public void CompleteSubTask(Guid subTaskId)
    {
        var index = _subTasks.FindIndex(st => st.Id == subTaskId);
        if (index >= 0)
        {
            _subTasks[index] = _subTasks[index].MarkAsDone();
        }
    }

    public void UncompleteSubTask(Guid subTaskId)
    {
        var index = _subTasks.FindIndex(st => st.Id == subTaskId);
        if (index >= 0)
        {
            _subTasks[index] = _subTasks[index].MarkAsUndone();
        }
    }

    public void UpdateSubTaskTitle(Guid subTaskId, string newTitle)
    {
        var index = _subTasks.FindIndex(st => st.Id == subTaskId);
        if (index >= 0)
        {
            _subTasks[index] = _subTasks[index].UpdateTitle(newTitle);
        }
    }

    // Computed properties
    public bool IsOverdue => DueDate.IsOverdue && Status != TodoStatus.Done;
    public bool IsDueToday => DueDate.IsDueToday;
    public bool IsDueTomorrow => DueDate.IsDueTomorrow;
    public bool IsDueThisWeek => DueDate.IsDueThisWeek;
    public bool HasSubTasks => _subTasks.Count > 0;
    public int CompletedSubTasksCount => _subTasks.Count(st => st.IsDone);
    public int TotalSubTasksCount => _subTasks.Count;

    public double SubTasksCompletionPercentage => HasSubTasks
        ? (double)CompletedSubTasksCount / TotalSubTasksCount * 100
        : 0;
}
