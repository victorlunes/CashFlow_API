using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENT_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE = 25;
    private readonly IExpensesReadOnlyRepository _repository; 
    
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0)
        {
            return [];
        }
        
        var document = CreateDocument(month);
        var page = CreatePage(document);
        
        CreateHeader(page);
        
        var totalExpenses = expenses.Sum(expense => expense.Amount);

        CreateTotalSpentSection(page, month, totalExpenses);
        
        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForValue(row.Cells[3]);
            
            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;
            
            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);
            
            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);
            
            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) is false)
            {
                var description = table.AddRow();
                description.Height = HEIGHT_ROW_EXPENSE;
                
                description.Cells[0].AddParagraph(expense.Description);
                description.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSAN_REGULAR, Size = 10};
                description.Cells[0].Shading.Color = Color.Parse("#FDFAFF");
                description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                description.Cells[0].MergeRight = 2;
                description.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }
        
        return RenderDocuments(document);
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE;
        row.Borders.Visible = false;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{CURRENT_SYMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSAN_REGULAR, Size = 14, Color = Colors.Black};
        cell.Shading.Color = Color.Parse("#FFFFFF");
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSAN_REGULAR, Size = 12, Color = Colors.Black};
        cell.Shading.Color = Color.Parse("#FCF2FF");
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddHeaderForValue(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = Colors.White};
        cell.Shading.Color = Color.Parse("#9A3AFC");
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14};
        cell.Shading.Color = Color.Parse("#DBB9FE");
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }
    
    private byte[] RenderDocuments(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();
        
        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        
        return file.ToArray();
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month.ToString(format: "Y")}";
        document.Info.Author = "Victor";
        
        var style = document.Styles["Normal"];
        style.Font.Name = FontHelper.RALEWAY_BLACK;
        
        return document;
    }
    private Section CreatePage(Document document)
    {
        var section =  document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.TopMargin = 70;
        section.PageSetup.BottomMargin = 70;
        
        return section;
    }

    private void CreateHeader(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();

        var row = table.AddRow();
        row.Cells[0].AddParagraph("CashFlow");
        row.Cells[0].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 18 };
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString(format: "Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR,  Size = 15 });
        
        paragraph.AddLineBreak();
        paragraph.AddFormattedText($"{totalExpenses} {CURRENT_SYMBOL}", new Font { Name = FontHelper.WORKSAN_BLACK, Size = 40 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.Borders.Visible = false;
        table.Format.Font.Size = 15;
        
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
        
        return table;
    }
}