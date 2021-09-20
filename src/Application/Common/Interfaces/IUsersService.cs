using System.Threading.Tasks;
using Application.Common.Models;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUsers;

namespace Application.Common.Interfaces
{
    /// <summary>
    ///     IUsersService interface
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        ///     Updates user
        /// </summary>
        /// <param name="request">Update user command</param>
        Task UpdateUserAsync(UpdateUserCommand request);

        /// <summary>
        ///     Deletes user
        /// </summary>
        /// <param name="request">Delete user command</param>
        /// <param name="adminAccess">Administrator access</param>
        Task DeleteUserAsync(DeleteUserCommand request, bool adminAccess);

        /// <summary>
        ///     Gets user
        /// </summary>
        /// <param name="request">Get user query</param>
        Task<UserDto> GetUserAsync(GetUserQuery request);

        /// <summary>
        ///     Gets users
        /// </summary>
        /// <param name="request">Get users query</param>
        Task<PaginatedList<UserDto>> GetUsersAsync(GetUsersQuery request);
    }
}