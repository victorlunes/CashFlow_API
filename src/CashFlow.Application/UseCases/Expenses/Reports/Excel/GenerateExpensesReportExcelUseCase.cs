using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;
    private const string CURRENT_SYMBOL = "R$";
    
    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0)
            return [];
        
        var workBook = new XLWorkbook();

        workBook.Author = "Victor";
        workBook.Style.Font.FontSize = 12;
        workBook.Style.Font.FontName = "Times New Roman";
        
        var workSheet = workBook.Worksheets.Add(month.ToString("Y"));
        
        InsertHeader(workSheet);

        var raw = 2;
        decimal total = 0;
        foreach (var expense in expenses)
        {
            workSheet.Cell($"A{raw}").Value = expense.Title;
            workSheet.Cell($"B{raw}").Value = expense.Date;
            workSheet.Cell($"C{raw}").Value = ConvertPaymentType(expense.PaymentType);
            workSheet.Cell($"D{raw}").Value = expense.Amount;
            workSheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENT_SYMBOL} #,##0.00";
            workSheet.Cell($"E{raw}").Value = expense.Description;
            
            total = total + expense.Amount;
            raw++;
        }
        
        workSheet.Cell($"D{raw}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        workSheet.Cell($"D{raw}").Value = total; 
        workSheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENT_SYMBOL} #,##0.00";
        
        workSheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workBook.SaveAs(file);
        
        return file.ToArray();
    }
    
    private string ConvertPaymentType(PaymentType paymentType)
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
    private void InsertHeader(IXLWorksheet workSheet)
    {
        workSheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        workSheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        workSheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        workSheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        workSheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        
        workSheet.Cells("A1:E1").Style.Font.Bold = true;
        workSheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.LightPastelPurple;
        
        workSheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        workSheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        
    }
}