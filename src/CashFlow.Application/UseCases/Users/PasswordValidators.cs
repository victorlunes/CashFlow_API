using System.Text.RegularExpressions;
using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.Users;

public class PasswordValidators<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE = "ErrorMessage";
    public override string Name => "PasswordValidators";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        if (Regex.IsMatch(password, @"[A-Z]+") == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        if (Regex.IsMatch(password, @"[a-z]+") == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        if (Regex.IsMatch(password, @"[0-9]+") == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        if (Regex.IsMatch(password, @"[\!\?\*\.]+") == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessage.PASSWORD_INVALID);
            return false;
        }

        return true;
    }
}