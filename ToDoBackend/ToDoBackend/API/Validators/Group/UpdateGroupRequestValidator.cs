using FluentValidation;
using ToDoBackend.DTO.Group;

namespace ToDoBackend.API.Validators.Group;

public class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
{
    /// <summary>
    /// Constructor that initializes rules for validation
    /// </summary>
    public UpdateGroupRequestValidator()
    {
        RuleFor(i => i.Name).NotEmpty().MaximumLength(40);
        RuleFor(i => i.Color)
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid hexadecimal color code. Example: #ABC123");
    }
}