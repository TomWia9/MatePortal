using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    /// <summary>
    /// Current user service
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        /// <summary>
        /// Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// User manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes CurrentUSerService
        /// </summary>
        /// <param name="httpContextAccessor">Http context accessor</param>
        /// <param name="userManager">User manager</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        /// <summary>
        /// Current user ID
        /// </summary>
        public Guid? UserId => GetCurrentUserId();

        /// <summary>
        /// Gets current user id
        /// </summary>
        /// <returns></returns>
        private Guid? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }

        /// <summary>
        /// Gets current user role
        /// </summary>
        /// <returns>Current user role</returns>
        public async Task<string> GetCurrentUserRoleAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            if (await _userManager.IsInRoleAsync(user, Roles.Administrator))
            {
                return Roles.Administrator;
            }

            if (await _userManager.IsInRoleAsync(user, Roles.User))
            {
                return Roles.User;
            }

            return null;
        }
    }
}