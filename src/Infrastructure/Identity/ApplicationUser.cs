using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}