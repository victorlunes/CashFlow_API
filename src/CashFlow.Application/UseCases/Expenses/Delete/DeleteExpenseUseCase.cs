using AutoMapper;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository expenseRepository, IUnitOfWork unitOfWork)
    {
        _repository = expenseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (result == false)
        {
            throw new NotFoundException(ResourceErrorMessage.EXPENSE_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}