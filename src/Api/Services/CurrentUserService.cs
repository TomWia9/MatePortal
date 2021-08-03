using System;
using System.Security.Claims;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;

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
        /// Initializes CurrentUSerService
        /// </summary>
        /// <param name="httpContextAccessor">Http context accessor</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Current user ID
        /// </summary>
        public Guid? UserId => GetCurrentUserId();

        public string UserRole => GetCurrentUserRole();

        /// <summary>
        /// Gets current user ID
        /// </summary>
        /// <returns>Current user ID</returns>
        private Guid? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }

        /// <summary>
        /// Gets current user role
        /// </summary>
        /// <returns>Current user role</returns>
        private string GetCurrentUserRole()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.User.IsInRole(Roles.User))
                {
                    return Roles.User;
                }

                if (_httpContextAccessor.HttpContext.User.IsInRole(Roles.Administrator))
                {
                    return Roles.Administrator;
                }
            }

            return null;
        }
    }
}