using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    /// <summary>
    /// Update user handler
    /// </summary>
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        /// <summary>
        /// User service
        /// </summary>
        private readonly IUsersService _userService;

        /// <summary>
        /// Initializes UpdateUserHandler
        /// </summary>
        /// <param name="userService">User service</param>
        public UpdateUserHandler(IUsersService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Handles updating user
        /// </summary>
        /// <param name="request">The update user request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(request);

            return Unit.Value;
        }
    }
}