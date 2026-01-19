namespace CashFlow.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public Guid UserIdentifier { get; set; } = Guid.Empty;
    public string Role { get; set; } = String.Empty;
}