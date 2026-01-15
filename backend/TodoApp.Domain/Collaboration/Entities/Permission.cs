using TodoApp.Domain.Common;
using TodoApp.Domain.Collaboration.Enums;

namespace TodoApp.Domain.Collaboration.Entities;

/// <summary>
/// Entity đại diện cho quyền truy cập vào resource
/// </summary>
public class Permission : AuditableEntity
{
    public ResourceType ResourceType { get; private set; }
    public Guid ResourceId { get; private set; }
    public SubjectType SubjectType { get; private set; }
    public Guid SubjectId { get; private set; } // UserId hoặc RoleId
    public PermissionLevel Level { get; private set; }

    private Permission() : base() { }

    public static Permission Create(
        ResourceType resourceType,
        Guid resourceId,
        SubjectType subjectType,
        Guid subjectId,
        PermissionLevel level)
    {
        return new Permission
        {
            ResourceType = resourceType,
            ResourceId = resourceId,
            SubjectType = subjectType,
            SubjectId = subjectId,
            Level = level
        };
    }

    public static Permission CreateForUser(ResourceType resourceType, Guid resourceId, Guid userId, PermissionLevel level)
    {
        return Create(resourceType, resourceId, SubjectType.User, userId, level);
    }

    public static Permission CreateForRole(ResourceType resourceType, Guid resourceId, Guid roleId, PermissionLevel level)
    {
        return Create(resourceType, resourceId, SubjectType.Role, roleId, level);
    }

    public void UpdateLevel(PermissionLevel newLevel)
    {
        Level = newLevel;
    }

    public bool CanRead => Level >= PermissionLevel.Read;
    public bool CanWrite => Level >= PermissionLevel.Write;
    public bool CanAdmin => Level >= PermissionLevel.Admin;
}
