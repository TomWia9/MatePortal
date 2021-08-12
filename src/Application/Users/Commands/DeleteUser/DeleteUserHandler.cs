using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.DeleteUser
{
    /// <summary>
    /// Delete user handler
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        /// <summary>
        /// User service
        /// </summary>
        private readonly IUsersService _userService;

        /// <summary>
        /// Current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes DeleteUserHandler
        /// </summary>
        /// <param name="userService">User service</param>
        /// <param name="currentUserService">Current user service</param>
        public DeleteUserHandler(IUsersService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles deleting user
        /// </summary>
        /// <param name="request">The delete user request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(request, _currentUserService.UserRole == "Administrator");

            return Unit.Value;
        }
    }
}