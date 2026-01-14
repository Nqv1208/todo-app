using TodoApp.Domain.Common;

namespace TodoApp.Domain.Events;

/// <summary>
/// Domain Event được raise khi một User mới đăng ký
/// </summary>
public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Username { get; }
    public string Email { get; }

    public UserRegisteredEvent(Guid userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
    }
}

