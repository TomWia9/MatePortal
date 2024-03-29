﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Enums;
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

namespace Infrastructure.Services;

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
    ///     Query service
    /// </summary>
    private readonly IQueryService<ApplicationUser> _queryService;

    /// <summary>
    ///     User manager
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     Initializes UsersService
    /// </summary>
    /// <param name="userManager">User manager</param>
    /// <param name="context">Database context</param>
    /// <param name="queryService">Query service</param>
    /// <param name="currentUserService">Current user service</param>
    public UsersService(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        IQueryService<ApplicationUser> queryService,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _context = context;
        _queryService = queryService;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Updates user
    /// </summary>
    /// <param name="request">Update user command</param>
    public async Task UpdateUserAsync(UpdateUserCommand request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        user.UserName = request.Username;

        var updateUserResult = await _userManager.UpdateAsync(user);

        if (!updateUserResult.Succeeded)
            throw new BadRequestException(string.Join(", ", updateUserResult.Errors.Select(x => x.Description)));

        //The administrator does not need to enter the user's current password to change their password
        if (_currentUserService.AdministratorAccess)
        {
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);

            if (!removePasswordResult.Succeeded)
                throw new BadRequestException(string.Join(", ",
                    removePasswordResult.Errors.Select(x => x.Description)));

            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);

            if (!addPasswordResult.Succeeded)
                throw new BadRequestException(string.Join(", ",
                    addPasswordResult.Errors.Select(x => x.Description)));
        }
        else
        {
            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!changePasswordResult.Succeeded)
                throw new BadRequestException(string.Join(", ",
                    changePasswordResult.Errors.Select(x => x.Description)));
        }
    }

    /// <summary>
    ///     Deletes user
    /// </summary>
    /// <param name="request">Delete user command</param>
    /// <param name="adminAccess">Administrator access</param>
    public async Task DeleteUserAsync(DeleteUserCommand request, bool adminAccess)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        if (!adminAccess && !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new BadRequestException("Provided password is incorrect");

        await _userManager.DeleteAsync(user);
    }

    /// <summary>
    ///     Gets user
    /// </summary>
    /// <param name="request">Get user query</param>
    /// <exception cref="NotFoundException">Throws when user is not found</exception>
    public async Task<UserDto> GetUserAsync(GetUserQuery request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        var userRoles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.UserName,
            Role = userRoles.FirstOrDefault()
        };
    }

    /// <summary>
    ///     Gets users
    /// </summary>
    /// <param name="request">Get users query</param>
    public async Task<PaginatedList<UserDto>> GetUsersAsync(GetUsersQuery request)
    {
        var collection = _context.Users.AsQueryable();

        //cant be added to predicates
        if (!string.IsNullOrEmpty(request.Parameters.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Parameters.Role);
            collection = usersInRole.AsQueryable();
        }

        var predicates = GetPredicates(request.Parameters);

        collection = _queryService.Search(collection, predicates);
        collection = Sort(collection, request.Parameters.SortBy, request.Parameters.SortDirection);

        var mappedUsers = await MapUsers(collection.ToList());
        return new PaginatedList<UserDto>(mappedUsers, mappedUsers.Count,
            request.Parameters.PageNumber,
            request.Parameters.PageSize);
    }

    /// <summary>
    ///     Maps collection of ApplicationUser to the list of UserDto
    /// </summary>
    /// <param name="users">The ApplicationUser collection</param>
    /// <returns>List of UserDto</returns>
    private async Task<IList<UserDto>> MapUsers(IEnumerable<ApplicationUser> users)
    {
        var mappedUsers = new List<UserDto>();

        foreach (var user in users)
        {
            var userRole = await _userManager.GetRolesAsync(user);
            mappedUsers.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Role = userRole.FirstOrDefault()
            });
        }

        return mappedUsers;
    }

    /// <summary>
    ///     Sorts users by yerba mate opinions count or shop opinions count.
    /// </summary>
    /// <param name="collection">The collection of users.</param>
    /// <param name="dbSet">The DbSet.</param>
    /// <param name="exp">The expression (ex. x => x.CreatedBy).</param>
    /// <param name="sortDirection">The sort direction.</param>
    /// <typeparam name="T">The type of dbSet.</typeparam>
    /// <returns>IQueryable of ApplicationUser sorted by number of specific opinions.</returns>
    private static IQueryable<ApplicationUser> SortByOpinionsCount<T>(IEnumerable<ApplicationUser> collection,
        IEnumerable<T> dbSet,
        Func<T, object> exp,
        SortDirection sortDirection) where T : class
    {
        var usersWithOpinionsCount = collection.ToList().GroupJoin(dbSet, x => x.Id, exp,
            (user, opinions) => new
            {
                User = user,
                OpinionsCount = opinions.Count()
            }).AsQueryable();

        usersWithOpinionsCount = sortDirection == SortDirection.Asc
            ? usersWithOpinionsCount.OrderBy(x => x.OpinionsCount)
            : usersWithOpinionsCount.OrderByDescending(x => x.OpinionsCount);

        return usersWithOpinionsCount.Select(x => x.User).AsQueryable();
    }

    /// <summary>
    ///     Gets filtering and searching predicates for users
    /// </summary>
    /// <param name="parameters">The users query parameters</param>
    /// <returns>The users predicates</returns>
    private static IEnumerable<Expression<Func<ApplicationUser, bool>>> GetPredicates(UsersQueryParameters parameters)
    {
        var predicates = new List<Expression<Func<ApplicationUser, bool>>>();

        if (string.IsNullOrWhiteSpace(parameters.SearchQuery))
            return predicates;

        var searchQuery = parameters.SearchQuery.Trim().ToLower();
        Expression<Func<ApplicationUser, bool>> searchPredicate =
            x => x.Email.ToLower().Contains(searchQuery) ||
                 x.UserName.ToLower().Contains(searchQuery);

        predicates.Add(searchPredicate);

        return predicates;
    }

    /// <summary>
    ///     Gets sorting column expression
    /// </summary>
    /// <param name="sortBy">Column by which to sort</param>
    /// <returns>The sorting expression</returns>
    private static Expression<Func<ApplicationUser, object>> GetSortingColumn(string sortBy)
    {
        var sortingColumns = new Dictionary<string, Expression<Func<ApplicationUser, object>>>
        {
            { nameof(ApplicationUser.Email).ToLower(), u => u.Email },
            { nameof(ApplicationUser.UserName).ToLower(), u => u.UserName }
        };

        return string.IsNullOrEmpty(sortBy) ? sortingColumns.First().Value : sortingColumns[sortBy.ToLower()];
    }

    /// <summary>
    ///     Sorts users by given column in given direction
    /// </summary>
    /// <param name="collection">The users collection</param>
    /// <param name="sortBy">Column by which to sort</param>
    /// <param name="sortDirection">Direction in which to sort</param>
    /// <returns>The sorted collection of users</returns>
    private IQueryable<ApplicationUser> Sort(IQueryable<ApplicationUser> collection, string sortBy,
        SortDirection sortDirection)
    {
        const string yerbaMateOpinions = "yerbamateopinions";
        const string shopOpinions = "shopopinions";

        if (!string.IsNullOrEmpty(sortBy) && sortBy.ToLower() is yerbaMateOpinions or shopOpinions)
        {
            collection = sortBy.ToLower() == yerbaMateOpinions
                ? SortByOpinionsCount(collection, _context.YerbaMateOpinions, x => x.CreatedBy,
                    sortDirection)
                : SortByOpinionsCount(collection, _context.ShopOpinions, x => x.CreatedBy,
                    sortDirection);
        }
        else
        {
            var sortingColumn = GetSortingColumn(sortBy);
            collection = _queryService.Sort(collection, sortingColumn, sortDirection);
        }

        return collection;
    }
}