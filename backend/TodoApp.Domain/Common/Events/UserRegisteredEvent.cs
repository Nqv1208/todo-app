namespace TodoApp.Domain.Common.Events;

// Domain Event khi user đăng ký thành công
public class UserRegisteredEvent : BaseEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string Name { get; }

    public UserRegisteredEvent(Guid userId, string email, string name)
    {
        UserId = userId;
        Email = email;
        Name = name;
    }
}
