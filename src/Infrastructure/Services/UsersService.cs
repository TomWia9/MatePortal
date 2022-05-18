using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUsers;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    /// <summary>
    ///     Users service
    /// </summary>
    public class UsersService : IUsersService
    {
        /// <summary>
        ///     Database context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        ///     Sort service
        /// </summary>
        private readonly ISortService<ApplicationUser> _sortService;

        /// <summary>
        ///     User manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        ///     Initializes UsersService
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="context">Database context</param>
        /// <param name="sortService">Sort service</param>
        public UsersService(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            ISortService<ApplicationUser> sortService)
        {
            _userManager = userManager;
            _context = context;
            _sortService = sortService;
        }

        /// <summary>
        ///     Updates user
        /// </summary>
        /// <param name="request">Update user command</param>
        public async Task UpdateUserAsync(UpdateUserCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            user.UserName = request.Username;
            await _userManager.UpdateAsync(user);
            await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        }

        /// <summary>
        ///     Deletes user
        /// </summary>
        /// <param name="request">Delete user command</param>
        /// <param name="adminAccess">Administrator access</param>
        public async Task DeleteUserAsync(DeleteUserCommand request, bool adminAccess)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (!adminAccess && !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new ForbiddenAccessException();

            await _userManager.DeleteAsync(user);
        }

        /// <summary>
        ///     Gets user
        /// </summary>
        /// <param name="request">Get user query</param>
        /// <exception cref="NotFoundException">Throws when user is not found</exception>
        public async Task<UserDto> GetUserAsync(GetUserQuery request)
        {
            var entity = await _context.Users.FindAsync(request.UserId);

            if (entity == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

            return new UserDto
            {
                Id = entity.Id,
                Email = entity.Email,
                Username = entity.UserName
            };
        }

        /// <summary>
        ///     Gets users
        /// </summary>
        /// <param name="request">Get users query</param>
        public async Task<PaginatedList<UserDto>> GetUsersAsync(GetUsersQuery request)
        {
            var collection = _context.Users.AsQueryable();

            //searching
            if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
            {
                var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

                collection = collection.Where(u => u.Email.ToLower().Contains(searchQuery)
                                                   || u.UserName.ToLower().Contains(searchQuery));
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(request.Parameters.SortBy))
            {
                var sortingColumns = new Dictionary<string, Expression<Func<ApplicationUser, object>>>
                {
                    {nameof(ApplicationUser.UserName), u => u.UserName},
                    {nameof(ApplicationUser.Email), u => u.Email}
                };

                collection = _sortService.Sort(collection, request.Parameters.SortBy,
                    request.Parameters.SortDirection, sortingColumns);

                return new PaginatedList<UserDto>(MapUsers(collection), MapUsers(collection).Count,
                    request.Parameters.PageNumber,
                    request.Parameters.PageSize);
            }

            //If sortBy is null, sort by email
            collection = collection.OrderBy(u => u.Email);
            return new PaginatedList<UserDto>(MapUsers(collection), MapUsers(collection).Count,
                request.Parameters.PageNumber,
                request.Parameters.PageSize);
        }

        private static IList<UserDto> MapUsers(IEnumerable<ApplicationUser> collection)
        {
            return collection.Select(user => new UserDto {Id = user.Id, Email = user.Email, Username = user.UserName})
                .ToList();
        }
    }
}