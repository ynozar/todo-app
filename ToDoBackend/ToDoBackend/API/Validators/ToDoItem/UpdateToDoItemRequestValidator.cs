using FluentValidation;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.API.Validators.ToDoItem;

public class UpdateToDoItemRequestValidator : AbstractValidator<UpdateToDoItemRequest>
{
    /// <summary>
    /// Constructor that initializes rules for validation
    /// </summary>
    public UpdateToDoItemRequestValidator()
    {
        RuleFor(i => i.Title).NotEmpty().MaximumLength(40);
        RuleFor(i => i.Priority).NotEmpty().InclusiveBetween(1,3);
        RuleFor(i => i.IsComplete).NotEmpty().InclusiveBetween(true,false);
    }
}