using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.UpdateUser;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Users.Commands
{
    /// <summary>
    /// Update user tests
    /// </summary>
    public class UpdateUserTests : IntegrationTest
    {
        [Fact]
        public async Task ShouldUpdateUserData()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            var command = new UpdateUserCommand
            {
                UserId = userId,
                Username = "UpdatedUsername",
                CurrentPassword = "Qwerty123_",
                NewPassword = "UpdatedPassword123_"
            };

            await _mediator.Send(command);

            var updatedUser = await AuthHelper.GetUserByTokenAsync(_factory, token);

            updatedUser.Id.Should().Be(userId);
            updatedUser.Email.Should().Be(user.Email);
            updatedUser.UserName.Should().Be(command.Username);
            updatedUser.PasswordHash.Should().NotBeSameAs(user.PasswordHash);
        }

        // //Todo Add validator
        // /// <summary>
        // /// Update user should throw when one or more properties are invalid
        // /// </summary>
        // [Theory]
        // [InlineData("", "password")]
        // [InlineData("username", "password")]
        // [InlineData("u", "Pa$$word!23")]
        // public async Task ShouldThrowWhenWhenPropertiesAreInvalid(string username, string newPassword)
        // {
        //     //First, register user
        //     var result = await AuthHelper.RegisterTestUserAsync(_mediator);
        //     var token = result.As<AuthSuccessResponse>().Token;
        //     var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
        //     var userId = user.Id;
        //
        //     var command = new UpdateUserCommand()
        //     {
        //         UserId = userId,
        //         Username = username,
        //         CurrentPassword = "Qwerty123_",
        //         NewPassword = newPassword
        //     };
        //
        //     FluentActions.Invoking(() =>
        //         _mediator.Send(command)).Should().BeOfType<ValidationException>();
        // }
    }
}