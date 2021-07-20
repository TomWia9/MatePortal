using System;
using MediatR;

namespace Application.Users.Queries.GetUser
{
    /// <summary>
    /// Get single user query
    /// </summary>
    public class GetUserQuery : IRequest<UserDto>
    {
        /// <summary>
        /// Initializes GetUserQuery
        /// </summary>
        /// <param name="userId">User ID</param>
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User ID
        /// </summary>
        private Guid UserId { get; }
    }
}