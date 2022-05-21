using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Users.Queries.GetUsers;

/// <summary>
///     Get users handler
/// </summary>
public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDto>>
{
    /// <summary>
    ///     Users service
    /// </summary>
    private readonly IUsersService _userService;

    /// <summary>
    ///     Initializes GetUsersHandler
    /// </summary>
    public GetUsersHandler(IUsersService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Handles getting users
    /// </summary>
    /// <param name="request">Get users request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of user data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        return await _userService.GetUsersAsync(request);
    }
}