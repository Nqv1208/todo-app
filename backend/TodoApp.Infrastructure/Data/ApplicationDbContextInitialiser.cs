using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;
using TodoApp.Domain.ValueObjects;

namespace TodoApp.Infrastructure.Data;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _logger = logger;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Skip if already seeded
        if (await _context.Users.AnyAsync())
        {
            _logger.LogInformation("Database already seeded. Skipping...");
            return;
        }

        _logger.LogInformation("Seeding database...");

        // 1. Create Users
        var adminUser = await SeedUsersAsync();

        // 2. Create Workspaces
        var (personalWorkspace, teamWorkspace) = await SeedWorkspacesAsync(adminUser.Id);

        // 3. Create Content (Pages, Todos)
        await SeedContentAsync(personalWorkspace.Id, teamWorkspace.Id, adminUser.Id);

        _logger.LogInformation("Database seeded successfully.");
    }

    private async Task<User> SeedUsersAsync()
    {
        // Admin user
        var admin = User.Create(
            email: "admin@todoapp.com",
            name: "Admin User",
            passwordHash: _passwordHasher.HashPassword("Admin@123")
        );

        // Demo user
        var demo = User.Create(
            email: "demo@todoapp.com",
            name: "Demo User",
            passwordHash: _passwordHasher.HashPassword("Demo@123")
        );

        // Test user
        var test = User.Create(
            email: "test@todoapp.com",
            name: "Test User",
            passwordHash: _passwordHasher.HashPassword("Test@123")
        );

        _context.Users.AddRange(admin, demo, test);
        await _context.SaveChangesAsync(default);

        _logger.LogInformation("Seeded 3 users: admin@todoapp.com, demo@todoapp.com, test@todoapp.com");
        
        return admin;
    }

    private async Task<(Workspace personal, Workspace team)> SeedWorkspacesAsync(Guid adminUserId)
    {
        // Personal workspace for admin
        var personalWorkspace = Workspace.CreatePersonal("My Workspace", adminUserId);

        // Team workspace
        var teamWorkspace = Workspace.CreateTeam(
            "Development Team", 
            adminUserId,
            "Workspace for development team collaboration"
        );

        _context.Workspaces.AddRange(personalWorkspace, teamWorkspace);
        await _context.SaveChangesAsync(default);

        _logger.LogInformation("Seeded 2 workspaces: Personal + Team");

        return (personalWorkspace, teamWorkspace);
    }

    private async Task SeedContentAsync(Guid personalWorkspaceId, Guid teamWorkspaceId, Guid userId)
    {
        // === PERSONAL WORKSPACE ===

        // 1. Getting Started page
        var gettingStartedPage = ContentItem.CreatePage(
            "🚀 Getting Started",
            personalWorkspaceId
        );
        gettingStartedPage.UpdateIcon(Icon.FromEmoji("🚀"));
        _context.ContentItems.Add(gettingStartedPage);

        // 2. My Tasks - Todo list
        var myTasksContent = ContentItem.CreateTodoList("📝 My Tasks", personalWorkspaceId);
        myTasksContent.UpdateIcon(Icon.FromEmoji("📝"));
        _context.ContentItems.Add(myTasksContent);

        // 3. Personal Notes page
        var notesPage = ContentItem.CreatePage("📓 Personal Notes", personalWorkspaceId);
        notesPage.UpdateIcon(Icon.FromEmoji("📓"));
        _context.ContentItems.Add(notesPage);

        // === TEAM WORKSPACE ===

        // 1. Project Roadmap
        var roadmapPage = ContentItem.CreatePage("🗺️ Project Roadmap", teamWorkspaceId);
        roadmapPage.UpdateIcon(Icon.FromEmoji("🗺️"));
        _context.ContentItems.Add(roadmapPage);

        // 2. Sprint Tasks
        var sprintTasks = ContentItem.CreateTodoList("🏃 Sprint Tasks", teamWorkspaceId);
        sprintTasks.UpdateIcon(Icon.FromEmoji("🏃"));
        _context.ContentItems.Add(sprintTasks);

        // 3. Team Wiki
        var wikiPage = ContentItem.CreatePage("📚 Team Wiki", teamWorkspaceId);
        wikiPage.UpdateIcon(Icon.FromEmoji("📚"));
        _context.ContentItems.Add(wikiPage);

        // Save all content items first
        await _context.SaveChangesAsync(default);

        // 4. Nested page - sub-page under Wiki
        var apiDocsPage = ContentItem.CreatePage("🔌 API Documentation", teamWorkspaceId, wikiPage.Id);
        apiDocsPage.UpdateIcon(Icon.FromEmoji("🔌"));
        _context.ContentItems.Add(apiDocsPage);
        await _context.SaveChangesAsync(default);

        // === ADD BLOCKS ===
        // Add blocks directly to context

        // Blocks for Getting Started page
        _context.Blocks.AddRange(
            Block.Create(gettingStartedPage.Id, BlockType.Heading1, "Welcome to TodoApp!", 0),
            Block.Create(gettingStartedPage.Id, BlockType.Text, "This is your personal workspace. Here you can create pages, todos, and organize your work.", 1),
            Block.Create(gettingStartedPage.Id, BlockType.Callout, "💡 Tip: Use the sidebar to navigate between pages and create new content.", 2)
        );

        // Blocks for Notes page
        _context.Blocks.AddRange(
            Block.Create(notesPage.Id, BlockType.Heading2, "Meeting Notes", 0),
            Block.Create(notesPage.Id, BlockType.Text, "Add your meeting notes here...", 1),
            Block.Create(notesPage.Id, BlockType.Divider, null, 2),
            Block.Create(notesPage.Id, BlockType.Heading2, "Ideas", 3),
            Block.Create(notesPage.Id, BlockType.BulletList, "New feature ideas", 4)
        );

        // Blocks for Roadmap page
        _context.Blocks.AddRange(
            Block.Create(roadmapPage.Id, BlockType.Heading1, "Q1 2026 Roadmap", 0),
            Block.Create(roadmapPage.Id, BlockType.Text, "Our goals and milestones for Q1 2026.", 1),
            Block.Create(roadmapPage.Id, BlockType.Heading2, "January", 2),
            Block.Create(roadmapPage.Id, BlockType.Checkbox, "Complete authentication system", 3),
            Block.Create(roadmapPage.Id, BlockType.Checkbox, "Implement workspace management", 4),
            Block.Create(roadmapPage.Id, BlockType.Heading2, "February", 5),
            Block.Create(roadmapPage.Id, BlockType.Checkbox, "Build content editor", 6),
            Block.Create(roadmapPage.Id, BlockType.Checkbox, "Add collaboration features", 7)
        );

        // Blocks for Wiki page
        _context.Blocks.AddRange(
            Block.Create(wikiPage.Id, BlockType.Heading1, "Team Documentation", 0),
            Block.Create(wikiPage.Id, BlockType.Text, "Welcome to our team wiki! Find all important documentation here.", 1),
            Block.Create(wikiPage.Id, BlockType.Divider, null, 2),
            Block.Create(wikiPage.Id, BlockType.Heading2, "Quick Links", 3),
            Block.Create(wikiPage.Id, BlockType.BulletList, "API Documentation", 4),
            Block.Create(wikiPage.Id, BlockType.BulletList, "Design System", 5),
            Block.Create(wikiPage.Id, BlockType.BulletList, "Deployment Guide", 6)
        );

        // Blocks for API Docs page
        _context.Blocks.AddRange(
            Block.Create(apiDocsPage.Id, BlockType.Heading1, "API Reference", 0),
            Block.Create(apiDocsPage.Id, BlockType.Text, "Complete API documentation for TodoApp.", 1),
            Block.Create(apiDocsPage.Id, BlockType.Code, "GET /api/v1/workspaces\nGET /api/v1/content-items\nPOST /api/v1/todos", 2)
        );

        // === ADD TODOS ===

        // Todo for My Tasks
        var myTasksTodo = Todo.Create(
            myTasksContent.Id,
            TodoPriority.Medium,
            DateTime.UtcNow.AddDays(7)
        );
        _context.Todos.Add(myTasksTodo);

        // Todo for Sprint Tasks
        var highPriorityTodo = Todo.Create(
            sprintTasks.Id,
            TodoPriority.High,
            DateTime.UtcNow.AddDays(3)
        );
        highPriorityTodo.AssignTo(userId);
        _context.Todos.Add(highPriorityTodo);

        // Save all blocks and todos
        await _context.SaveChangesAsync(default);

        _logger.LogInformation("Seeded content: Pages, Todos, Blocks");
    }
}
