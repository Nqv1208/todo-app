using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Content.Entities;

namespace TodoApp.Infrastructure.Data.Configurations;

public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.ToTable("blocks");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.ContentItemId)
            .HasColumnName("content_item_id")
            .IsRequired();

        builder.Property(b => b.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.Properties)
            .HasColumnName("properties")
            .HasColumnType("jsonb")
            .HasDefaultValue("{}");

        builder.Property(b => b.Position)
            .HasColumnName("position")
            .HasDefaultValue(0);

        builder.Property(b => b.ParentBlockId)
            .HasColumnName("parent_block_id");

        // Audit fields
        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(b => b.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(b => b.UpdatedBy)
            .HasColumnName("updated_by");

        // Self-referencing relationship (nested blocks)
        builder.HasOne(b => b.ParentBlock)
            .WithMany(b => b.Children)
            .HasForeignKey(b => b.ParentBlockId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(b => b.ContentItemId);
        builder.HasIndex(b => b.ParentBlockId);
        builder.HasIndex(b => new { b.ContentItemId, b.Position });

        // Ignore domain events
        builder.Ignore(b => b.DomainEvents);
    }
}
