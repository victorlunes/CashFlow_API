using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")] //letra maiúscula
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = new PasswordValidators<RequestRegisterUseJson>();
        
        var result = 
            validator.IsValid( new ValidationContext<RequestRegisterUseJson>( new RequestRegisterUseJson()), password);
        
        Assert.False(result);
    }
}