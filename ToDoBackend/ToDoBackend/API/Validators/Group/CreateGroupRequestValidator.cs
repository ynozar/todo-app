using FluentValidation;
using ToDoBackend.DTO.Group;

namespace ToDoBackend.API.Validators.Group;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
    /// <summary>
    /// Constructor that initializes rules for validation
    /// </summary>
    public CreateGroupRequestValidator()
    {
        RuleFor(i => i.Name).NotEmpty().MaximumLength(40);
        RuleFor(i => i.Color)
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid hexadecimal color code. Example: #ABC123");
    }
}