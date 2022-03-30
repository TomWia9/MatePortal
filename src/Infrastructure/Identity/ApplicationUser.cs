using System;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    /// <summary>
    /// The application user
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}