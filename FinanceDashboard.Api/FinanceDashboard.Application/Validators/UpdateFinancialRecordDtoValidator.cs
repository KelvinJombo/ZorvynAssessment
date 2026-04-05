using FinanceDashboard.Application.DTOs.Record;
using FluentValidation;

public class UpdateFinancialRecordDtoValidator : AbstractValidator<UpdateFinancialRecordDto>
{
    public UpdateFinancialRecordDtoValidator()
    {
        // Amount
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        // Type (Enum validation)
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid record type");

        // Category
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(100)
            .WithMessage("Category must not exceed 100 characters");

        // Date
        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date cannot be in the future");

        // Notes
        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}