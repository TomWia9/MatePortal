using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.RegisterUser;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Users.Commands;

/// <summary>
///     Register new user tests
/// </summary>
public class RegisterUserTests : IntegrationTest
{
    /// <summary>
    ///     Register user should create user and return auth response
    /// </summary>
    [Fact]
    public async Task ShouldCreateUserAndReturnAuthResponse()
    {
        var command = new RegisterUserCommand
        {
            Email = "test@test.com",
            Username = "Test",
            Password = "Qwerty123_"
        };

        var result = await Mediator.Send(command);

        var jwt = result.Token;
        jwt.Should().NotBeNullOrEmpty();

        var user = await AuthHelper.GetUserByTokenAsync(Factory, jwt);

        user.Email.Should().Be(command.Email);
        user.UserName.Should().Be(command.Username);
        user.PasswordHash.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    ///     User should require unique email
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueEmail()
    {
        await AuthHelper.RegisterTestUserAsync(Mediator);

        var command = new RegisterUserCommand
        {
            Email = "test@test.com",
            Username = "Test",
            Password = "Qwerty123_"
        };

        var result = await Mediator.Send(command);

        result.Success.Should().BeFalse();
        result.Token.Should().BeNullOrEmpty();
        result.ErrorMessages.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    ///     Register user should return error messages when one or more properties are invalid
    /// </summary>
    [Theory]
    [InlineData("email.com", "us", "password")]
    [InlineData("email@.com", "us", "Password")]
    [InlineData("adgsg", "user", "P")]
    public async Task ShouldReturnErrorMessagesWhenPropertiesAreInvalid(string email, string username,
        string password)
    {
        var command = new RegisterUserCommand
        {
            Email = email,
            Username = username,
            Password = password
        };

        var result = await Mediator.Send(command);

        result.Success.Should().BeFalse();
        result.Token.Should().BeNullOrEmpty();
        result.ErrorMessages.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    ///     Register user should return token when properties are valid
    /// </summary>
    [Theory]
    [InlineData("email@test.com", "user", "Password123_")]
    [InlineData("email@gmail.com", "User", "Test123!@fa")]
    [InlineData("EMAIL@TEST.COM", "user", "Password!!!!123_")]
    public async Task ShouldReturnTokenWhenPropertiesAreValid(string email, string username, string password)
    {
        var command = new RegisterUserCommand
        {
            Email = email,
            Username = username,
            Password = password
        };

        var result = await Mediator.Send(command);

        result.Success.Should().BeTrue();
        result.Token.Should().NotBeNullOrEmpty();
        result.ErrorMessages.Should().BeNullOrEmpty();
    }
}