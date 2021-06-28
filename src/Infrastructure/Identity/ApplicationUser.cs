using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public IList<Opinion> Opinions { get; set; } = new List<Opinion>();
        public IList<Favourite> Favourites { get; set; } = new List<Favourite>();
    }
}