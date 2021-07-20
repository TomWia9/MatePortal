using Application.Common.Models;
using MediatR;

namespace Application.Users.Queries.GetUsers
{
    /// <summary>
    /// Get all users query
    /// </summary>
    public class GetUsersQuery : IRequest<PaginatedList<UserDto>>
    {
        /// <summary>
        /// Initializes GetUsersQuery
        /// </summary>
        /// <param name="parameters">Users query parameters</param>
        public GetUsersQuery(UsersQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Users query parameters
        /// </summary>
        private UsersQueryParameters Parameters { get; }
    }
}