using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// Current user service interface
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Current user ID
        /// </summary>
        Guid? UserId { get; }
        
        /// <summary>
        /// Gets current user role
        /// </summary>
        /// <returns>Current user role</returns>
        Task<string> GetCurrentUserRoleAsync();
    }
}