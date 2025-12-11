using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterExpenseUseCase( IExpenseRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        validate(request);

        var entity = new Expense
        {
            Title = request.Title,
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            PaymentType = (Domain.Enums.PaymentType)request.PaymentType,
        };
        
        _repository.Add(entity);
        
        _unitOfWork.Commit();
        
        return new ResponseRegisteredExpenseJson();
    }

    private void validate(RequestRegisterExpenseJson request)
    {
        var validator = new RegisterExpenseValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}