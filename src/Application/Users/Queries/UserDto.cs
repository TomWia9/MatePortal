using System;

namespace Application.Users.Queries
{
    //TODO Map ApplicationUser from infrastructure project to UserDto from Application project
    //Or map this manually, or move ApplicationUser from Infrastructure project to Domain project
    //it looks like manual mapping in handler will be the best solution
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}