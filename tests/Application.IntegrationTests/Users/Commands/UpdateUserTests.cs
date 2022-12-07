using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.UpdateUser;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Users.Commands;

/// <summary>
///     Update user tests
/// </summary>
public class UpdateUserTests : IntegrationTest
{
    [Fact]
    public async Task ShouldUpdateUserData()
    {
        //First, register user
        var result = await AuthHelper.RegisterTestUserAsync(Mediator);
        var token = result.Token;
        var user = await AuthHelper.GetUserByTokenAsync(Factory, token);
        var userId = user.Id;
        
        Factory.CurrentUserId = user.Id;

        var command = new UpdateUserCommand
        {
            UserId = userId,
            Username = "UpdatedUsername",
            CurrentPassword = "Qwerty123_",
            NewPassword = "UpdatedPassword123_"
        };

        await Mediator.Send(command);

        var updatedUser = await AuthHelper.GetUserByTokenAsync(Factory, token);

        updatedUser.Id.Should().Be(userId);
        updatedUser.Email.Should().Be(user.Email);
        updatedUser.UserName.Should().Be(command.Username);
        updatedUser.PasswordHash.Should().NotBeSameAs(user.PasswordHash);
    }
}