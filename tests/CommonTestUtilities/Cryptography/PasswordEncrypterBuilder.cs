using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;
    
    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();

        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!%asdagdfb231");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrEmpty(password))
            return this;
        
        _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password,  It.IsAny<string>())).Returns(true);
        
        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;
}