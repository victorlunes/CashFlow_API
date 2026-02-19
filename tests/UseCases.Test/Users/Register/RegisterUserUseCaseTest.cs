using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Request;
using CommonTestUtilities.Token;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();
        
        var result = await useCase.Execute(request);
        
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();
        
        Func<Task> act = async () => await useCase.Execute(request);

        var result = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

        Assert.Single(result.GetErrors());
        
        Assert.Contains(ResourceErrorMessage.NAME_EMPTY, result.GetErrors());
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);
        
        Func<Task> act = async () => await useCase.Execute(request);
        
        var result = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        
        Assert.Single(result.GetErrors());
        
        Assert.Contains(ResourceErrorMessage.EMAIL_ALREADY_EXISTS, result.GetErrors());
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();
        if (string.IsNullOrWhiteSpace(email) == false)
        {
            readRepository.ExistActiveUserWithEmail(email);
        }
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        var jwtGenerator = JwtTokenGeneratorBuilder.Build();
        
        return new RegisterUserUseCase(mapper: mapper, passwordEncripter: passwordEncripter, userReadOnlyRepository: readRepository.Build(),  userWriteOnlyRepository: writeOnlyRepository,  unitOfWork: unitOfWork,   tokenGenerator: jwtGenerator);
    }
}