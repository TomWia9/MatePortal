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
        public Guid UserId { get; set; }

        /// <summary>
        ///     User's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     User's current password
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        ///     User's new password
        /// </summary>
        public string NewPassword { get; set; }
    }
}