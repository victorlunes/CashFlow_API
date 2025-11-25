using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Request;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]

    public void Success()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder.Build();
        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;
        
        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessage.TITLE_REQUIRED);
    }
    
    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);
        
        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessage.EXPENSES_CANNOT_FOR_THE_FUTURE);
    }
    
    [Fact]
    public void Error_PaymentType_Invalid()
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)8;
        
        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessage.PAYMENT_TYPE_INVALID);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-7)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arrange
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;
        
        //Act
        var result = validator.Validate(request);

        //Assert
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.ErrorMessage == ResourceErrorMessage.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
    }
}