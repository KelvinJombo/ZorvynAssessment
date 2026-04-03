using FinanceDashboard.Application.DTOs;
using FluentValidation;

namespace FinanceDashboard.Application.Validators
{
    public class CreateFinancialRecordValidator : AbstractValidator<CreateFinancialRecordDto>
    {
        public CreateFinancialRecordValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Type)
                .NotEmpty()
                .Must(x => x == "Income" || x == "Expense");

            RuleFor(x => x.Category)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }

}
