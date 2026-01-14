using TodoApp.Domain.Common;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Entity đại diện cho người dùng trong hệ thống
/// </summary>
public class User : AuditableEntity
{
    public string Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? DisplayName { get; private set; }
    public string? AvatarUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    // Navigation properties
    private readonly List<TodoList> _todoLists = new();
    public IReadOnlyCollection<TodoList> TodoLists => _todoLists.AsReadOnly();

    private readonly List<Label> _labels = new();
    public IReadOnlyCollection<Label> Labels => _labels.AsReadOnly();

    private User() : base() { }

    public static User Create(string username, string email, string passwordHash, string? displayName = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username không được để trống", nameof(username));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash không được để trống", nameof(passwordHash));

        var user = new User
        {
            Username = username.Trim().ToLowerInvariant(),
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            DisplayName = displayName?.Trim(),
            IsActive = true
        };

        return user;
    }

    public void UpdateProfile(string? displayName, string? avatarUrl)
    {
        DisplayName = displayName?.Trim();
        AvatarUrl = avatarUrl?.Trim();
    }

    public void UpdateEmail(string email)
    {
        Email = Email.Create(email);
    }

    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash không được để trống", nameof(passwordHash));

        PasswordHash = passwordHash;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void AddTodoList(TodoList todoList)
    {
        _todoLists.Add(todoList);
    }

    public void AddLabel(Label label)
    {
        _labels.Add(label);
    }
}

