using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessage.TITLE_REQUIRED);
        
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorMessage.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessage.EXPENSES_CANNOT_FOR_THE_FUTURE);
        
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessage.PAYMENT_TYPE_INVALID);
    }

}