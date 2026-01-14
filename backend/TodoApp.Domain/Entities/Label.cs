using TodoApp.Domain.Common;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Entity đại diện cho nhãn/tag để phân loại TodoItem
/// </summary>
public class Label : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public Color Color { get; private set; } = null!;

    // Foreign key
    public Guid UserId { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;

    private readonly List<TodoItem> _todoItems = new();
    public IReadOnlyCollection<TodoItem> TodoItems => _todoItems.AsReadOnly();

    private Label() : base() { }

    public static Label Create(string name, Guid userId, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên nhãn không được để trống", nameof(name));

        var label = new Label
        {
            Name = name.Trim(),
            Color = color != null ? Color.Create(color) : Color.Default,
            UserId = userId
        };

        return label;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên nhãn không được để trống", nameof(name));

        Name = name.Trim();
    }

    public void UpdateColor(string color)
    {
        Color = Color.Create(color);
    }

    public void AddTodoItem(TodoItem todoItem)
    {
        if (!_todoItems.Contains(todoItem))
        {
            _todoItems.Add(todoItem);
        }
    }

    public void RemoveTodoItem(TodoItem todoItem)
    {
        _todoItems.Remove(todoItem);
    }
}

