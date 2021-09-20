using System.Collections.Generic;

namespace Application.Users
{
    /// <summary>
    ///     Authentication result
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        ///     Authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Indicates success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///     Error messages list
        /// </summary>
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}