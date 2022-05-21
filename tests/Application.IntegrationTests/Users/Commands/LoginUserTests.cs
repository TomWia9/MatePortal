using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users;
using Application.Users.Commands.LoginUser;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Users.Commands;

/// <summary>
///     Login user tests
/// </summary>
public class LoginUserTests : IntegrationTest
{
    /// <summary>
    ///     Login user should return auth response
    /// </summary>
    [Fact]
    public async Task ShouldReturnAuthResponse()
    {
        await AuthHelper.RegisterTestUserAsync(Mediator);

        var command = new LoginUserCommand
        {
            Email = "test@test.com",
            Password = "Qwerty123_"
        };

        var result = await Mediator.Send(command);

        result.Should().BeOfType<AuthenticationResult>();
        var jwt = result.Token;
        jwt.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    ///     Login user should return error messages when one or more properties are invalid
    /// </summary>
    [Theory]
    [InlineData("test.com", "test")]
    [InlineData("test@test.com", "test")]
    [InlineData("test.com", "Test123_")]
    public async Task ShouldReturnErrorMessagesWhenPropertiesAreInvalid(string email, string password)
    {
        await AuthHelper.RegisterTestUserAsync(Mediator);

        var command = new LoginUserCommand
        {
            Email = email,
            Password = password
        };

        var result = await Mediator.Send(command);

        result.Success.Should().BeFalse();
        result.Token.Should().BeNullOrEmpty();
        result.ErrorMessages.Should().NotBeNullOrEmpty();
    }
}