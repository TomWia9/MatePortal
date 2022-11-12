using FluentValidation;

namespace Application.Users.Commands.DeleteUser;

/// <summary>
///     DeleteUserCommand validator
/// </summary>
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    ///     Initializes DeleteUserCommandValidator
    /// </summary>
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(256);
    }
}