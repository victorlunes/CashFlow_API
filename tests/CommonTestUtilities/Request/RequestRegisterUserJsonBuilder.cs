using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Request;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUseJson Build()
    {
        return new Faker<RequestRegisterUseJson>()
            .RuleFor(user => user.Name,
                faker => faker.Person.FullName)
            .RuleFor(user => user.Email,
                (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password,
                faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}