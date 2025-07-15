using FluentValidation;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.API.Validators.ToDoItem;

public class PatchToDoItemRequestValidator : AbstractValidator<PatchToDoItemRequest>
{
    /// <summary>
    /// Constructor that initializes rules for validation
    /// </summary>
    public PatchToDoItemRequestValidator()
    {
        /*
        RuleFor(i => i.Title).MaximumLength(40);
        RuleFor(i => i.Priority).InclusiveBetween(1,3);
        RuleFor(i => i.IsComplete).InclusiveBetween(true,false);
        */
    }
}