using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]

    public void Success()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = new RequestExpenseJson
        {
            Amount = 100,
            Date = DateTime.Now.AddDays(-1),
            Description = "descrição",
            PaymentType = CashFlow.Communication.Enums.PaymentType.DebitCard,
            Title = "Apple"
        };

        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
    }
}