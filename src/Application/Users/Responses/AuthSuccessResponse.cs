namespace Application.Users.Responses
{
    public class AuthSuccessResponse : IAuthResponse
    {
        public string Token { get; set; }
    }
}