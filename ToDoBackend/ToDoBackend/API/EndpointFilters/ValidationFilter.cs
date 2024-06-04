using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ToDoBackend.API.EndpointFilters;

public class ValidationFilter<T> : IEndpointFilter
{

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        IValidator<T>? validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is null)
        {
            return Results.Problem($"Could not find validator for {typeof(T)}");
        }

        T? argument = context.Arguments.OfType<T>().FirstOrDefault(i => i?.GetType() == typeof(T));

        if (argument is null)
        {
            return Results.Problem($"Could not find instance of {typeof(T)} to validate");
        }

        ValidationResult validation = await validator.ValidateAsync(argument);

        if (validation.IsValid)
        {
            return await next(context);
        }

        return Results.ValidationProblem(validation.ToDictionary());
    }
}