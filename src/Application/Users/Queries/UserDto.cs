using System;

namespace Application.Users.Queries
{
    //TODO Map ApplicationUser from infrastructure project to UserDto from Application project
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}