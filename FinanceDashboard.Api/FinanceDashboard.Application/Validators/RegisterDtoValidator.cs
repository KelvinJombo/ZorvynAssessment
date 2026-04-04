using FinanceDashboard.Application.DTOs.Auth;
using FinanceDashboard.Domain.Models.Enums;
using FluentValidation;

namespace FinanceDashboard.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Role)
                .Must(r => r == UserRole.Viewer || r == UserRole.Analyst || r == UserRole.Admin)
                .WithMessage("Invalid role");
        }
    }
}
