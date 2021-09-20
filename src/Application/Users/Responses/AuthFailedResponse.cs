using System.Collections.Generic;

namespace Application.Users.Responses
{
    /// <summary>
    ///     Auth failed response
    /// </summary>
    public class AuthFailedResponse : IAuthResponse
    {
        /// <summary>
        ///     Error messages list
        /// </summary>
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}