using FluentValidation;

namespace TodoApp.Application.Features.Auth.Commands.RefreshToken;

// Validator cho RefreshTokenCommand
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token không được để trống");
    }
}
