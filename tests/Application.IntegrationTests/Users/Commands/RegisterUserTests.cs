using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.RegisterUser;
using Application.Users.Responses;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Application.IntegrationTests.Users.Commands
{
    /// <summary>
    /// Register new user tests
    /// </summary>
    public class RegisterUserTests : IntegrationTest
    {
        /// <summary>
        /// Register user should create user and return auth response
        /// </summary>
        [Fact]
        public async Task ShouldCreateUserAndReturnAuthResponse()
        {
            var command = new RegisterUserCommand()
            {
               Email = "test@test.com",
               Username = "Test",
               Password = "Qwerty123_"
            };

            var result = await _mediator.Send(command);

            result.Should().BeOfType<AuthSuccessResponse>();
            var jwt = result.As<AuthSuccessResponse>().Token;
            jwt.Should().NotBeNullOrEmpty();

            var user = await AuthHelper.GetUserByTokenAsync(_factory, jwt);

            user.Email.Should().Be(command.Email);
            user.UserName.Should().Be(command.Username);
            user.PasswordHash.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// User should require unique email
        /// </summary>
        [Fact]
        public async Task ShouldRequireUniqueEmail()
        {
            await AuthHelper.RegisterTestUserAsync(_mediator);

            var command = new RegisterUserCommand()
            {
                Email = "test@test.com",
                Username = "Test",
                Password = "Qwerty123_"
            };
            
            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().BeOfType<BadRequestObjectResult>();
        }
        
        /// <summary>
        /// Register user should throw when one or more properties are invalid
        /// </summary>
        [Theory]
        [InlineData("email.com", "us", "password")]
        [InlineData("email@.com", "us", "Password")]
        [InlineData("email@@email.com", "user", "Password123_")]
        public void ShouldThrowWhenPropertiesAreInvalid(string email, string username, string password)
        {
            var command = new RegisterUserCommand()
            {
                Email = email,
                Username = username,
                Password = password
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().BeOfType<BadRequestObjectResult>();
        }
        
        /// <summary>
        /// Register user should not throw when properties are valid
        /// </summary>
        [Theory]
        [InlineData("email@test.com", "user", "Password123_")]
        [InlineData("email@gmail.com", "User", "123___!dasfa")]
        [InlineData("EMAIL@TEST.COM", "user", "Password!!!!123_")]
        public void ShouldNotThrowWhenPropertiesAreValid(string email, string username, string password)
        {
            var command = new RegisterUserCommand()
            {
                Email = email,
                Username = username,
                Password = password
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().BeOfType<BadRequestObjectResult>();
        }
    }
}