using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CashFlow.Exception;

namespace CashFlow.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CashFlowException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    public void HandleProjectException(ExceptionContext context)
    {
        var cashFlowException = (CashFlowException)context.Exception;
        var errorResponse = new ResponseErrorJson(cashFlowException.GetErrors());
        

        context.HttpContext.Response.StatusCode = cashFlowException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }
    
    public void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessage.UNKNOWN_ERROR);
        context.Result = new ObjectResult(errorResponse);
    }
}