using System.Collections.Generic;

namespace Application.Users.Responses
{
    public class AuthFailedResponse : IAuthResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}