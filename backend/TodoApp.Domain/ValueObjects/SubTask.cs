using TodoApp.Domain.Common;

namespace TodoApp.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho subtask của Todo
/// </summary>
public class SubTask : ValueObject
{
    public Guid Id { get; }
    public string Title { get; }
    public bool IsDone { get; }

    private SubTask(Guid id, string title, bool isDone)
    {
        Id = id;
        Title = title;
        IsDone = isDone;
    }

    public static SubTask Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề subtask không được để trống", nameof(title));

        return new SubTask(Guid.NewGuid(), title.Trim(), false);
    }

    public SubTask MarkAsDone() => new(Id, Title, true);
    
    public SubTask MarkAsUndone() => new(Id, Title, false);
    
    public SubTask UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Tiêu đề subtask không được để trống", nameof(newTitle));

        return new SubTask(Id, newTitle.Trim(), IsDone);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    public override string ToString() => $"[{(IsDone ? "x" : " ")}] {Title}";
}
