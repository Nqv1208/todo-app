using TodoApp.Domain.Common;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho icon (emoji hoặc icon name)
/// </summary>
public class Icon : ValueObject
{
    public string Value { get; }
    public IconType Type { get; }

    private Icon(string value, IconType type)
    {
        Value = value;
        Type = type;
    }

    public static Icon FromEmoji(string emoji)
    {
        if (string.IsNullOrWhiteSpace(emoji))
            throw new ArgumentException("Emoji không được để trống", nameof(emoji));

        return new Icon(emoji.Trim(), IconType.Emoji);
    }

    public static Icon FromName(string iconName)
    {
        if (string.IsNullOrWhiteSpace(iconName))
            throw new ArgumentException("Icon name không được để trống", nameof(iconName));

        return new Icon(iconName.Trim().ToLowerInvariant(), IconType.IconName);
    }

    public static Icon Default => FromEmoji("📝");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }

    public override string ToString() => Value;
}
