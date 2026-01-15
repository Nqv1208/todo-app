using TodoApp.Domain.Common;

namespace TodoApp.Domain.Content.ValueObjects;

/// <summary>
/// Value Object đại diện cho ngày hết hạn của Todo
/// </summary>
public class DueDate : ValueObject
{
    public DateTime? Value { get; }
    public bool HasDueDate => Value.HasValue;

    private DueDate(DateTime? value) => Value = value;

    public static DueDate Create(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value.Kind != DateTimeKind.Utc)
        {
            dueDate = DateTime.SpecifyKind(dueDate.Value, DateTimeKind.Utc);
        }
        return new DueDate(dueDate);
    }

    public static DueDate None => new(null);

    public bool IsOverdue => HasDueDate && Value!.Value < DateTime.UtcNow;
    public bool IsDueToday => HasDueDate && Value!.Value.Date == DateTime.UtcNow.Date;
    public bool IsDueTomorrow => HasDueDate && Value!.Value.Date == DateTime.UtcNow.Date.AddDays(1);

    public bool IsDueThisWeek
    {
        get
        {
            if (!HasDueDate) return false;
            var today = DateTime.UtcNow.Date;
            var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
            return Value!.Value.Date >= today && Value!.Value.Date <= endOfWeek;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value ?? DateTime.MinValue;
    }

    public override string ToString() => Value?.ToString("yyyy-MM-dd") ?? "No due date";
}
