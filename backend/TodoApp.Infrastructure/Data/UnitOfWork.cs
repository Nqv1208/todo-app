using Microsoft.EntityFrameworkCore.Storage;
using TodoApp.Domain.Common.Interfaces;
using TodoApp.Domain.Identity.Entities;
using TodoApp.Domain.Content.Entities;
using TodoApp.Domain.Collaboration.Entities;
using TodoApp.Domain.Activity.Entities;
using TodoApp.Domain.Workspace.Entities;
using TodoApp.Infrastructure.Data.Repositories;
using WorkspaceEntity = TodoApp.Domain.Workspace.Entities.Workspace;

namespace TodoApp.Infrastructure.Data;

// Unit of Work Implementation - Quản lý transaction và repositories
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    // Lazy-loaded repositories
    private IRepository<User>? _users;
    private IRepository<Session>? _sessions;
    private IRepository<WorkspaceEntity>? _workspaces;
    private IRepository<WorkspaceMember>? _workspaceMembers;
    private IRepository<ContentItem>? _contentItems;
    private IRepository<Block>? _blocks;
    private IRepository<Todo>? _todos;
    private IRepository<Permission>? _permissions;
    private IRepository<Comment>? _comments;
    private IRepository<ActivityLog>? _activityLogs;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // Identity
    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<Session> Sessions => _sessions ??= new Repository<Session>(_context);

    // Workspace
    public IRepository<WorkspaceEntity> Workspaces => _workspaces ??= new Repository<WorkspaceEntity>(_context);
    public IRepository<WorkspaceMember> WorkspaceMembers => _workspaceMembers ??= new Repository<WorkspaceMember>(_context);

    // Content
    public IRepository<ContentItem> ContentItems => _contentItems ??= new Repository<ContentItem>(_context);
    public IRepository<Block> Blocks => _blocks ??= new Repository<Block>(_context);
    public IRepository<Todo> Todos => _todos ??= new Repository<Todo>(_context);

    // Collaboration
    public IRepository<Permission> Permissions => _permissions ??= new Repository<Permission>(_context);
    public IRepository<Comment> Comments => _comments ??= new Repository<Comment>(_context);

    // Activity
    public IRepository<ActivityLog> ActivityLogs => _activityLogs ??= new Repository<ActivityLog>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
