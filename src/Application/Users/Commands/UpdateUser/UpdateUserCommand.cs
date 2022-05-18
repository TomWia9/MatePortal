using System;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    /// <summary>
    ///     Update user command
    /// </summary>
    public class UpdateUserCommand : IRequest
    {
        /// <summary>
        ///     User's ID
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        ///     User's username
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        ///     User's current password
        /// </summary>
        public string CurrentPassword { get; init; }

        /// <summary>
        ///     User's new password
        /// </summary>
        public string NewPassword { get; init; }
    }
}