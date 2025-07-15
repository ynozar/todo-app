using FluentValidation;
using ToDoBackend.DTO.Group;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.API.Validators.ToDoItem;

public class CreateToDoItemRequestValidator : AbstractValidator<CreateToDoItemRequest>
{
    /// <summary>
    /// Constructor that initializes rules for validation
    /// </summary>
    public CreateToDoItemRequestValidator()
    {
        RuleFor(i => i.Title).NotEmpty().MaximumLength(40);
        RuleFor(i => i.Priority).NotEmpty().InclusiveBetween(1,3);
        RuleFor(i => i.IsComplete).Must(x => !x || x);
    }
}