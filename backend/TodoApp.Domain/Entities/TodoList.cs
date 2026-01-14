using TodoApp.Domain.Common;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Entity đại diện cho danh sách chứa các TodoItem
/// </summary>
public class TodoList : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Color Color { get; private set; } = null!;
    public int Position { get; private set; }
    public bool IsArchived { get; private set; }

    // Foreign key
    public Guid UserId { get; private set; }
    
    // Navigation properties
    public User User { get; private set; } = null!;

    private readonly List<TodoItem> _items = new();
    public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

    private TodoList() : base() { }

    public static TodoList Create(string name, Guid userId, string? description = null, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên danh sách không được để trống", nameof(name));

        var todoList = new TodoList
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            Color = color != null ? Color.Create(color) : Color.Default,
            UserId = userId,
            Position = 0,
            IsArchived = false
        };

        return todoList;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên danh sách không được để trống", nameof(name));

        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void UpdateColor(string color)
    {
        Color = Color.Create(color);
    }

    public void UpdatePosition(int position)
    {
        if (position < 0)
            throw new ArgumentException("Vị trí không được âm", nameof(position));

        Position = position;
    }

    public void Archive()
    {
        IsArchived = true;
    }

    public void Unarchive()
    {
        IsArchived = false;
    }

    public void AddItem(TodoItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(TodoItem item)
    {
        _items.Remove(item);
    }

    public int GetCompletedItemsCount()
    {
        return _items.Count(i => i.Status == Enums.TodoStatus.Completed);
    }

    public int GetPendingItemsCount()
    {
        return _items.Count(i => i.Status == Enums.TodoStatus.Pending || i.Status == Enums.TodoStatus.InProgress);
    }
}

