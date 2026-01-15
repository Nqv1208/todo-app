using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Content.ValueObjects;
using System.Text.Json;

namespace TodoApp.Infrastructure.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("todos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id");

        builder.Property(t => t.ContentItemId)
            .HasColumnName("content_item_id")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Priority)
            .HasColumnName("priority")
            .HasConversion<string>()
            .HasMaxLength(50);

        // DueDate as owned type (Value Object)
        builder.OwnsOne(t => t.DueDate, dueDate =>
        {
            dueDate.Property(d => d.Value)
                .HasColumnName("due_date");
        });

        builder.Property(t => t.AssigneeId)
            .HasColumnName("assignee_id");

        builder.Property(t => t.CompletedAt)
            .HasColumnName("completed_at");

        // SubTasks as JSON column
        builder.Property<string>("_subTasksJson")
            .HasColumnName("sub_tasks")
            .HasColumnType("jsonb")
            .HasDefaultValue("[]");

        // Audit fields
        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(t => t.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(t => t.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(t => t.UpdatedBy)
            .HasColumnName("updated_by");

        // Indexes
        builder.HasIndex(t => t.ContentItemId)
            .IsUnique();

        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.AssigneeId);
        builder.HasIndex(t => t.Priority);

        // Ignore collection navigation (handled via JSON)
        builder.Ignore(t => t.SubTasks);

        // Ignore domain events
        builder.Ignore(t => t.DomainEvents);
    }
}
