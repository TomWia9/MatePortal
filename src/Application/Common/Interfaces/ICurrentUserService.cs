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
        /// Current user role
        /// </summary>
        string UserRole { get; }
    }
}