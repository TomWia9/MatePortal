using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    /// <summary>
    /// Register user command
    /// </summary>
    public class RegisterUserCommand : IRequest<IAuthResponse>
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; init; }
        /// <summary>
        /// User's username
        /// </summary>
        public string Username { get; init; }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; init; }
    }
}