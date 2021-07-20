using Application.Users.Queries;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    /// <summary>
    /// Create user command
    /// </summary>
    public class CreateUserCommand : IRequest<UserDto>
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User's username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
    }
}