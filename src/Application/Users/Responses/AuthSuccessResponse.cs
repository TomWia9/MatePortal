namespace Application.Users.Responses
{
    /// <summary>
    /// Auth success response
    /// </summary>
    public class AuthSuccessResponse : IAuthResponse
    {
        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; }
    }
}