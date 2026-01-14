using System.Text.RegularExpressions;
using TodoApp.Domain.Common;

namespace TodoApp.Domain.ValueObjects;

/// <summary>
/// Value Object đại diện cho màu sắc (HEX format)
/// </summary>
public partial class Color : ValueObject
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Color Create(string hexColor)
    {
        if (string.IsNullOrWhiteSpace(hexColor))
            throw new ArgumentException("Màu không được để trống", nameof(hexColor));

        hexColor = hexColor.Trim().ToUpperInvariant();

        if (!hexColor.StartsWith('#'))
            hexColor = $"#{hexColor}";

        if (!HexColorRegex().IsMatch(hexColor))
            throw new ArgumentException("Màu không hợp lệ. Định dạng yêu cầu: #RRGGBB hoặc #RGB", nameof(hexColor));

        return new Color(hexColor);
    }

    public static Color Default => new("#3B82F6"); // Blue

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Color color) => color.Value;

    [GeneratedRegex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", RegexOptions.Compiled)]
    private static partial Regex HexColorRegex();
}

