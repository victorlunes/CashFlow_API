using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Request;
using FluentValidation;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]

    public void Success()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        Assert.True(result.IsValid);

    }
    
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceErrorMessage.NAME_EMPTY, result.Errors[0].ErrorMessage);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceErrorMessage.EMAIL_EMPTY, result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "victor.com";
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceErrorMessage.EMAIL_INVALID, result.Errors[0].ErrorMessage);
    }
    
    
    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceErrorMessage.PASSWORD_INVALID, result.Errors[0].ErrorMessage);
    }
}