using FinanceDashboard.Application.DTOs.Record;
using FluentValidation;

namespace FinanceDashboard.Application.Validators
{
    public class FinancialRecordFilterDtoValidator : AbstractValidator<FinancialRecordFilterDto>
    {
        public FinancialRecordFilterDtoValidator()
        {
            // Date range validation
            RuleFor(x => x)
                .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate)
                .WithMessage("StartDate must be less than or equal to EndDate");

            // Category validation
            RuleFor(x => x.Category)
                .MaximumLength(100)
                .WithMessage("Category must not exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Category));

            // Amount range validation
            RuleFor(x => x)
                .Must(x => !x.MinAmount.HasValue || !x.MaxAmount.HasValue || x.MinAmount <= x.MaxAmount)
                .WithMessage("MinAmount must be less than or equal to MaxAmount");

            // Ensure amounts are not negative
            RuleFor(x => x.MinAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MinAmount cannot be negative")
                .When(x => x.MinAmount.HasValue);

            RuleFor(x => x.MaxAmount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxAmount cannot be negative")
                .When(x => x.MaxAmount.HasValue);

            // Pagination validation (inherited from PaginationParams)
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50)
                .WithMessage("PageSize must be between 1 and 50");
        }
    }
}
