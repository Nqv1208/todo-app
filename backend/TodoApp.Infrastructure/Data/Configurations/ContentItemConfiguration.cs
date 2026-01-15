using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Content.Entities;

namespace TodoApp.Infrastructure.Data.Configurations;

public class ContentItemConfiguration : IEntityTypeConfiguration<ContentItem>
{
    public void Configure(EntityTypeBuilder<ContentItem> builder)
    {
        builder.ToTable("content_items");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(500)
            .IsRequired();

        // Icon as owned type (Value Object)
        builder.OwnsOne(c => c.Icon, icon =>
        {
            icon.Property(i => i.Value)
                .HasColumnName("icon_value")
                .HasMaxLength(100);

            icon.Property(i => i.Type)
                .HasColumnName("icon_type")
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        builder.Property(c => c.Cover)
            .HasColumnName("cover")
            .HasMaxLength(500);

        builder.Property(c => c.ParentId)
            .HasColumnName("parent_id");

        builder.Property(c => c.WorkspaceId)
            .HasColumnName("workspace_id")
            .IsRequired();

        builder.Property(c => c.Position)
            .HasColumnName("position")
            .HasDefaultValue(0);

        builder.Property(c => c.IsArchived)
            .HasColumnName("is_archived")
            .HasDefaultValue(false);

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at");

        // Audit fields
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by");

        // Self-referencing relationship (parent-children)
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationships
        builder.HasMany(c => c.Blocks)
            .WithOne(b => b.ContentItem)
            .HasForeignKey(b => b.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Todo)
            .WithOne(t => t.ContentItem)
            .HasForeignKey<Todo>(t => t.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.WorkspaceId);
        builder.HasIndex(c => c.ParentId);
        builder.HasIndex(c => c.Type);
        builder.HasIndex(c => new { c.WorkspaceId, c.IsDeleted });

        // Query filter for soft delete
        builder.HasQueryFilter(c => !c.IsDeleted);

        // Ignore domain events
        builder.Ignore(c => c.DomainEvents);
    }
}
