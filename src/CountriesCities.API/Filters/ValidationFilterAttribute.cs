using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;

namespace CountriesCities.API.Filters;

public class ValidationFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validatableArguments = context.ActionArguments.Values.Where(v => v != null);

        foreach (var argument in validatableArguments)
        {
            var argumentType = argument!.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(argumentType);
                var validationContext = Activator.CreateInstance(validationContextType, argument) as IValidationContext;
                
                var validationResult = await validator.ValidateAsync(validationContext!);
                
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }
        }

        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var errorResponse = new CountriesCities.Application.Common.ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Message = "One or more validation errors occurred.",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}
