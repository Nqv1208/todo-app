using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Common;
using TodoApp.Domain.Identity.Entities;
using TodoApp.Domain.Workspace.Entities;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Collaboration.Entities;
using TodoApp.Domain.Activity.Entities;

namespace TodoApp.Infrastructure.Data;

// Application Database Context - EF Core DbContext
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<Session> Sessions => Set<Session>();

    // Workspace
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();

    // Content
    public DbSet<ContentItem> ContentItems => Set<ContentItem>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<Todo> Todos => Set<Todo>();

    // Collaboration
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Comment> Comments => Set<Comment>();

    // Activity
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-update audit fields
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
