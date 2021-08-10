using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.LoginUser;
using Application.Users.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Application.IntegrationTests.Users.Commands
{
    /// <summary>
    /// Login user tests
    /// </summary>
    public class LoginUserTests : IntegrationTest
    {
        /// <summary>
        /// Login user should return auth response
        /// </summary>
        [Fact]
        public async Task ShouldReturnAuthResponse()
        {
            await AuthHelper.RegisterTestUserAsync(_mediator);
            
            var command = new LoginUserCommand()
            {
                Email = "test@test.com",
                Password = "Qwerty123_"
            };

            var result = await _mediator.Send(command);

            result.Should().BeOfType<AuthSuccessResponse>();
            var jwt = result.As<AuthSuccessResponse>().Token;
            jwt.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Login user should throw when one or more properties are invalid
        /// </summary>
        [Theory]
        [InlineData("test.com", "test")]
        [InlineData("test@test.com", "test")]
        [InlineData("test.com", "Test123_")]
        public async Task ShouldThrowWhenWhenPropertiesAreInvalid(string email, string password)
        {
            await AuthHelper.RegisterTestUserAsync(_mediator);

            var command = new LoginUserCommand()
            {
                Email = email,
                Password = password
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().BeOfType<BadRequestObjectResult>();
        }
    }
}