namespace TodoApp.Domain.Exceptions;

/// <summary>
/// Base exception cho tất cả các lỗi domain
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception khi không tìm thấy entity
/// </summary>
public class EntityNotFoundException : DomainException
{
    public string EntityName { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityName, object entityId)
        : base($"Entity '{entityName}' với ID '{entityId}' không tồn tại.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}

/// <summary>
/// Exception khi có lỗi validation trong domain
/// </summary>
public class DomainValidationException : DomainException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public DomainValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public DomainValidationException(string propertyName, string error)
        : base($"Validation failed for '{propertyName}': {error}")
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { error } }
        };
    }

    public DomainValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}

/// <summary>
/// Exception khi vi phạm business rule
/// </summary>
public class BusinessRuleViolationException : DomainException
{
    public string RuleName { get; }

    public BusinessRuleViolationException(string ruleName, string message)
        : base(message)
    {
        RuleName = ruleName;
    }
}

