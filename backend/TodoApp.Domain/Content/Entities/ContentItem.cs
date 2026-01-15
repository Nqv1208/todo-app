namespace TodoApp.Domain.Content.Entities;

// Aggregate Root - Mọi thứ đều là Content (giống Notion)
// ContentItem có thể là Page, Todo, hoặc Database
public class ContentItem : AuditableEntity
{
    public ContentType Type { get; private set; }
    public string Title { get; private set; } = null!;
    public Icon Icon { get; private set; } = null!;
    public string? Cover { get; private set; }
    public Guid? ParentId { get; private set; }
    public Guid WorkspaceId { get; private set; }
    public int Position { get; private set; }
    public bool IsArchived { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation
    public ContentItem? Parent { get; private set; }
    
    private readonly List<ContentItem> _children = new();
    public IReadOnlyCollection<ContentItem> Children => _children.AsReadOnly();

    private readonly List<Block> _blocks = new();
    public IReadOnlyCollection<Block> Blocks => _blocks.AsReadOnly();

    // Todo specific - only populated when Type == ContentType.Todo
    public Todo? Todo { get; private set; }

    private ContentItem() : base() { }

    public static ContentItem CreatePage(string title, Guid workspaceId, Guid? parentId = null)
    {
        return Create(ContentType.Page, title, workspaceId, parentId, Icon.FromEmoji("📄"));
    }

    public static ContentItem CreateTodoList(string title, Guid workspaceId, Guid? parentId = null)
    {
        return Create(ContentType.Todo, title, workspaceId, parentId, Icon.FromEmoji("✅"));
    }

    public static ContentItem CreateDatabase(string title, Guid workspaceId, Guid? parentId = null)
    {
        return Create(ContentType.Database, title, workspaceId, parentId, Icon.FromEmoji("🗃️"));
    }

    private static ContentItem Create(ContentType type, string title, Guid workspaceId, Guid? parentId, Icon icon)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề không được để trống", nameof(title));

        return new ContentItem
        {
            Type = type,
            Title = title.Trim(),
            Icon = icon,
            WorkspaceId = workspaceId,
            ParentId = parentId,
            Position = 0,
            IsArchived = false,
            IsDeleted = false
        };
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề không được để trống", nameof(title));

        Title = title.Trim();
    }

    public void UpdateIcon(Icon icon)
    {
        Icon = icon ?? throw new ArgumentNullException(nameof(icon));
    }

    public void UpdateCover(string? coverUrl)
    {
        Cover = coverUrl?.Trim();
    }

    public void UpdatePosition(int position)
    {
        if (position < 0)
            throw new ArgumentException("Vị trí không được âm", nameof(position));

        Position = position;
    }

    public void MoveTo(Guid? newParentId)
    {
        ParentId = newParentId;
    }

    public void MoveToWorkspace(Guid newWorkspaceId)
    {
        WorkspaceId = newWorkspaceId;
    }

    public void Archive()
    {
        IsArchived = true;
    }

    public void Unarchive()
    {
        IsArchived = false;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }

    public Block AddBlock(BlockType blockType, string? content = null, int? position = null)
    {
        var block = Block.Create(Id, blockType, content, position ?? _blocks.Count);
        _blocks.Add(block);
        return block;
    }

    public void RemoveBlock(Guid blockId)
    {
        var block = _blocks.FirstOrDefault(b => b.Id == blockId);
        if (block != null)
        {
            _blocks.Remove(block);
            ReorderBlocks();
        }
    }

    public void ReorderBlocks()
    {
        var orderedBlocks = _blocks.OrderBy(b => b.Position).ToList();
        for (int i = 0; i < orderedBlocks.Count; i++)
        {
            orderedBlocks[i].UpdatePosition(i);
        }
    }

    public void AddChild(ContentItem child)
    {
        if (child.WorkspaceId != WorkspaceId)
            throw new InvalidOperationException("Child phải thuộc cùng workspace");

        child.ParentId = Id;
        _children.Add(child);
    }

    public void AttachTodo(Todo todo)
    {
        if (Type != ContentType.Todo)
            throw new InvalidOperationException("Chỉ ContentItem loại Todo mới có thể attach Todo");

        Todo = todo;
    }
}
