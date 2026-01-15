namespace TodoApp.Domain.Workspace.Entities;

/// <summary>
/// Entity đại diện cho không gian làm việc (Personal hoặc Team)
/// </summary>
public class Workspace : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public WorkspaceType Type { get; private set; }
    public Guid OwnerId { get; private set; }
    public Icon Icon { get; private set; } = null!;
    public bool IsArchived { get; private set; }

    // Navigation - Members
    private readonly List<WorkspaceMember> _members = new();
    public IReadOnlyCollection<WorkspaceMember> Members => _members.AsReadOnly();

    private Workspace() : base() { }

    public static Workspace CreatePersonal(string name, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên workspace không được để trống", nameof(name));

        var workspace = new Workspace
        {
            Name = name.Trim(),
            Type = WorkspaceType.Personal,
            OwnerId = ownerId,
            Icon = Icon.Default,
            IsArchived = false
        };

        // Owner tự động là member với role Owner
        workspace._members.Add(WorkspaceMember.Create(workspace.Id, ownerId, MemberRole.Owner));

        return workspace;
    }

    public static Workspace CreateTeam(string name, Guid ownerId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên workspace không được để trống", nameof(name));

        var workspace = new Workspace
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            Type = WorkspaceType.Team,
            OwnerId = ownerId,
            Icon = Icon.FromEmoji("👥"),
            IsArchived = false
        };

        workspace._members.Add(WorkspaceMember.Create(workspace.Id, ownerId, MemberRole.Owner));

        return workspace;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên workspace không được để trống", nameof(name));

        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void UpdateIcon(Icon icon)
    {
        Icon = icon ?? throw new ArgumentNullException(nameof(icon));
    }

    public void Archive() => IsArchived = true;
    public void Unarchive() => IsArchived = false;

    public WorkspaceMember AddMember(Guid userId, MemberRole role = MemberRole.Member)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new InvalidOperationException("User đã là thành viên của workspace");

        if (Type == WorkspaceType.Personal && role != MemberRole.Guest)
            throw new InvalidOperationException("Personal workspace chỉ cho phép thêm Guest");

        var member = WorkspaceMember.Create(Id, userId, role);
        _members.Add(member);
        return member;
    }

    public void RemoveMember(Guid userId)
    {
        if (userId == OwnerId)
            throw new InvalidOperationException("Không thể xóa Owner khỏi workspace");

        var member = _members.FirstOrDefault(m => m.UserId == userId);
        if (member != null)
        {
            _members.Remove(member);
        }
    }

    public void UpdateMemberRole(Guid userId, MemberRole newRole)
    {
        if (userId == OwnerId && newRole != MemberRole.Owner)
            throw new InvalidOperationException("Không thể thay đổi role của Owner");

        var member = _members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new InvalidOperationException("User không phải là thành viên của workspace");

        member.UpdateRole(newRole);
    }

    public bool IsMember(Guid userId) => _members.Any(m => m.UserId == userId);

    public MemberRole? GetMemberRole(Guid userId) => 
        _members.FirstOrDefault(m => m.UserId == userId)?.Role;

    public bool CanUserEdit(Guid userId)
    {
        var role = GetMemberRole(userId);
        return role is MemberRole.Owner or MemberRole.Admin or MemberRole.Member;
    }

    public bool CanUserAdmin(Guid userId)
    {
        var role = GetMemberRole(userId);
        return role is MemberRole.Owner or MemberRole.Admin;
    }
}
