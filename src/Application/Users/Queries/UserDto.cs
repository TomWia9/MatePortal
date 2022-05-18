using System;

namespace Application.Users.Queries
{
    //TODO Map ApplicationUser from infrastructure project to UserDto from Application project
    //Or map this manually, or move ApplicationUser from Infrastructure project to Domain project
    //it looks like manual mapping in handler will be the best solution
    //another idea is to move UserDto to Infrastructure project

    /// <summary>
    ///     User data transfer object
    /// </summary>
    public class UserDto
    {
        /// <summary>
        ///     User's ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     User's email
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        ///     User's username
        /// </summary>
        public string Username { get; init; }
    }
}