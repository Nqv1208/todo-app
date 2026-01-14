namespace TodoApp.Domain.Common;

/// <summary>
/// Base entity với audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)
/// </summary>
public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }

    protected AuditableEntity() : base()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected AuditableEntity(Guid id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }
}

