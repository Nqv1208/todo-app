using TodoApp.Domain.Common;
using TodoApp.Domain.Collaboration.Enums;

namespace TodoApp.Domain.Collaboration.Entities;

/// <summary>
/// Entity đại diện cho comment trên resource
/// </summary>
public class Comment : AuditableEntity
{
    public Guid Id { get; private set; } // Add this line for Id property
    public ResourceType ResourceType { get; private set; }
    public Guid ResourceId { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; } = null!;
    public Guid? ParentCommentId { get; private set; }
    public bool IsEdited { get; private set; }
    public DateTime? EditedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navigation
    public Comment? ParentComment { get; private set; }

    private readonly List<Comment> _replies = new();
    public IReadOnlyCollection<Comment> Replies => _replies.AsReadOnly();

    private Comment() : base() { }

    public static Comment Create(ResourceType resourceType, Guid resourceId, Guid userId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Nội dung comment không được để trống", nameof(content));

        return new Comment
        {
            ResourceType = resourceType,
            ResourceId = resourceId,
            UserId = userId,
            Content = content.Trim(),
            IsEdited = false,
            IsDeleted = false
        };
    }

    public Comment Reply(Guid userId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Nội dung reply không được để trống", nameof(content));

        var reply = new Comment
        {
            ResourceType = ResourceType,
            ResourceId = ResourceId,
            UserId = userId,
            Content = content.Trim(),
            ParentCommentId = ParentComment?.Id,
            IsEdited = false,
            IsDeleted = false
        };

        _replies.Add(reply);
        return reply;
    }

    public void Edit(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
            throw new ArgumentException("Nội dung comment không được để trống", nameof(newContent));

        Content = newContent.Trim();
        IsEdited = true;
        EditedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        Content = "[Đã xóa]";
    }
}
