using FluentValidation;

namespace TodoApp.Application.Features.Auth.Commands.Register;

// Validator cho RegisterCommand
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự")
            .MaximumLength(100).WithMessage("Mật khẩu không được vượt quá 100 ký tự")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ hoa")
            .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ thường")
            .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 số")
            .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống")
            .MinimumLength(2).WithMessage("Tên phải có ít nhất 2 ký tự")
            .MaximumLength(100).WithMessage("Tên không được vượt quá 100 ký tự");
    }
}
