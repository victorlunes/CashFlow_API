using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Requests;

public class RequestRegisterExpenseJson
{
    public string Title { get; set; } =  String.Empty;
    
    public string Description { get; set; } = String.Empty;
    
    public DateTime Date { get; set; } = DateTime.Now;
    
    public decimal Amount { get; set; }
    
    public PaymentType PaymentType { get; set; }
}