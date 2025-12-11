using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.Extensions.Logging;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository: IExpenseRepository
{
    private readonly CashFlowDbContext _dbContext;
    
    public ExpensesRepository(CashFlowDbContext dbContext, ILogger<ExpensesRepository> logger)
    {
        _dbContext = dbContext;
    }
    public void Add(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
    }
    
}