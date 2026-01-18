using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourcePaymentType.CASH,
            PaymentType.CreditCard => ResourcePaymentType.CREDIT_CARD,
            PaymentType.DebitCard => ResourcePaymentType.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourcePaymentType.ELETRONIC_TRANSFER,
            _ => string.Empty,
        };
    }
}