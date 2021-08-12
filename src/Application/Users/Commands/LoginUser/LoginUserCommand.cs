using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
    /// <summary>
    /// Login user command
    /// </summary>
    public class LoginUserCommand : IRequest<AuthenticationResult>
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; init; }
    }
}