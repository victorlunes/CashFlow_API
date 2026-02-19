using CashFlow.Application.UseCases.Users.Login.DoLogin;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request.Login;
using CommonTestUtilities.Token;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Builder();
        
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        
        var useCase = CreateUseCase(user, request.Password);
        
        var result = await useCase.Execute(request);
        
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Builder();
        
        var request = RequestLoginJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Password);
        
        Func<Task> act = async () => await useCase.Execute(request);
        
        var result = await Assert.ThrowsAsync<InvalidLoginException>(act);
        
        Assert.Single(result.GetErrors());
        
        Assert.Contains(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID, result.GetErrors());
    }

    [Fact]
    public async Task Error_Password_Not_Math()
    {
        var user = UserBuilder.Builder();
        
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;
        
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);
        
        var result = await Assert.ThrowsAsync<InvalidLoginException>(act);
        
        Assert.Single(result.GetErrors());
        
        Assert.Contains(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID, result.GetErrors());
    }

    private DoLoginUseCase CreateUseCase(CashFlow.Domain.Entities.User user, string? password = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
        var jwtGenerator = JwtTokenGeneratorBuilder.Build();

        return new DoLoginUseCase(repository: readRepository, passwordEncripter: passwordEncripter,
            accessTokenGenerator: jwtGenerator);
    }
}