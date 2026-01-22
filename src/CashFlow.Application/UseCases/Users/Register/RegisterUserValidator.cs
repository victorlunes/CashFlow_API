using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUseJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessage.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessage.EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceErrorMessage.EMAIL_EMPTY);

        RuleFor(user => user.Password).SetValidator(new PasswordValidators<RequestRegisterUseJson>());
    }
}