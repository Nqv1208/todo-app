namespace TodoApp.Domain.Common;

/// <summary>
/// Interface cho Domain Event - dùng để publish events khi có thay đổi trong domain
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

/// <summary>
/// Base class cho Domain Event
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}

