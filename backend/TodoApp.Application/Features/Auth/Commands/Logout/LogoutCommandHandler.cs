using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Application.Common.Models;

namespace TodoApp.Application.Features.Auth.Commands.Logout;

// Command đăng xuất (thu hồi refresh token)
public record LogoutCommand : IRequest<Result>
{
    public required string RefreshToken { get; init; }
}

// Handler cho LogoutCommand
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public LogoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.RefreshToken == request.RefreshToken, cancellationToken);

        if (session is not null)
        {
            session.Revoke();
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Luôn trả về success (không tiết lộ token có tồn tại hay không)
        return Result.Success();
    }
}
