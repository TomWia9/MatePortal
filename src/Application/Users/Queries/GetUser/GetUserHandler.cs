using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Queries.GetUser
{
    /// <summary>
    ///     Get user handler
    /// </summary>
    public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUsersService _usersService;

        /// <summary>
        ///     Initializes GetUserHandler
        /// </summary>
        public GetUserHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        ///     Handles getting user
        /// </summary>
        /// <param name="request">Get user request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>User data transfer object</returns>
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _usersService.GetUserAsync(request);
        }
    }
}