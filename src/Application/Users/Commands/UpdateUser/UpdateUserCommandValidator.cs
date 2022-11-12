using FluentValidation;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
///     UpdateUserCommand validator
/// </summary>
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    /// <summary>
    ///     Initializes UpdateUserCommandValidator
    /// </summary>
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .NotEqual(x => x.CurrentPassword)
            .MaximumLength(256);
    }
}