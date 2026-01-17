using TodoApp.Domain.Common;
using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities;

// Entity đại diện cho Block (Notion-style)
// Block chứa nội dung trong ContentItem
public class Block : AuditableEntity
{
    public Guid ContentItemId { get; private set; }
    public BlockType Type { get; private set; }
    public string Properties { get; private set; } = "{}"; // JSON
    public int Position { get; private set; }
    public Guid? ParentBlockId { get; private set; }

    // Navigation
    public ContentItem ContentItem { get; private set; } = null!;
    public Block? ParentBlock { get; private set; }

    private readonly List<Block> _children = new();
    public IReadOnlyCollection<Block> Children => _children.AsReadOnly();

    private Block() : base() { }

    public static Block Create(Guid contentItemId, BlockType type, string? content = null, int position = 0)
    {
        var block = new Block
        {
            ContentItemId = contentItemId,
            Type = type,
            Position = position
        };

        if (!string.IsNullOrWhiteSpace(content))
        {
            block.SetContent(content);
        }

        return block;
    }

    public void SetContent(string content)
    {
        // Simple text content stored in JSON format
        Properties = System.Text.Json.JsonSerializer.Serialize(new { text = content });
    }

    public string? GetContent()
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(Properties);
            if (doc.RootElement.TryGetProperty("text", out var textElement))
            {
                return textElement.GetString();
            }
        }
        catch
        {
            // Invalid JSON, return null
        }
        return null;
    }

    public void SetProperties(string jsonProperties)
    {
        // Validate JSON
        try
        {
            System.Text.Json.JsonDocument.Parse(jsonProperties);
            Properties = jsonProperties;
        }
        catch (System.Text.Json.JsonException)
        {
            throw new ArgumentException("Properties phải là JSON hợp lệ", nameof(jsonProperties));
        }
    }

    public void UpdateType(BlockType newType)
    {
        Type = newType;
    }

    internal void UpdatePosition(int position)
    {
        if (position < 0)
            throw new ArgumentException("Vị trí không được âm", nameof(position));

        Position = position;
    }

    public void AddChild(Block childBlock)
    {
        childBlock.ParentBlockId = Id;
        _children.Add(childBlock);
    }

    public void RemoveChild(Block childBlock)
    {
        _children.Remove(childBlock);
        childBlock.ParentBlockId = null;
    }
}
