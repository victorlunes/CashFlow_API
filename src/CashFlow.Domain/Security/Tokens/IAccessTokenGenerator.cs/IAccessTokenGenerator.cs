using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens.IAccessTokenGenerator.cs;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}