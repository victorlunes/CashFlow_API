using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Responses;

public class ResponseExpenseJson
{
    public long id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}