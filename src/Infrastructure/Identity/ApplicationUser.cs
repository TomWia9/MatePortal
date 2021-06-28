using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}