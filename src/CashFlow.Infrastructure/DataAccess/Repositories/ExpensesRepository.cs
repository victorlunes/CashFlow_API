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
    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }
    
}