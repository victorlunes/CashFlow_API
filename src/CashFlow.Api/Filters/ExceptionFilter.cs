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
        if (context.Exception is ErrorOnValidationException errorOnValidationException)
        {
            var errorResponse = new ResponseErrorJson(errorOnValidationException.Errors);
            
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
        else if (context.Exception is NotFoundException notFoundException)
        {
            var errorResponse = new ResponseErrorJson(notFoundException.Message);
            
            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Result = new NotFoundObjectResult(errorResponse);
        }
        else
        {
            var errorResponse = new ResponseErrorJson(context.Exception.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }
    
    public void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessage.UNKNOWN_ERROR);
        context.Result = new ObjectResult(errorResponse);
    }
}