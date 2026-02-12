using CashFlow.Application.UseCases.Users.Register;
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

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var jwtGenerator = JwtTokenGeneratorBuilder.Build();
        
        return new RegisterUserUseCase(mapper: mapper, passwordEncripter:passwordEncripter, userReadOnlyRepository:readRepository,  userWriteOnlyRepository:writeOnlyRepository,  unitOfWork: unitOfWork,   tokenGenerator:jwtGenerator);
    }
}