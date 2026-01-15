namespace TodoApp.Domain.Common;

// Interface cho Domain Event - dùng để publish events khi có thay đổi trong domain
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

// Base class cho Domain Event
public abstract class BaseEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }

    protected BaseEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}

