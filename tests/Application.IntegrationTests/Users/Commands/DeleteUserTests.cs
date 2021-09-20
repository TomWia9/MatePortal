using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.DeleteUser;
using FluentAssertions;
using Infrastructure.Identity;
using Xunit;

namespace Application.IntegrationTests.Users.Commands
{
    public class DeleteUserTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldBeAbleToDeleteAccount()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            _factory.CurrentUserId = userId;
            _factory.CurrentUserRole = Roles.User;

            await _mediator.Send(new DeleteUserCommand { UserId = userId, Password = "Qwerty123_" });

            user = await AuthHelper.GetUserByTokenAsync(_factory, token);

            user.Should().BeNull();
        }

        [Fact]
        public async Task UserShouldNotBeAbleToDeleteAccountWhenProvidedPasswordIsInvalid()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteUserCommand { UserId = userId, Password = "123" }))
                .Should()
                .Throw<ForbiddenAccessException>();
        }

        [Fact]
        public async Task AdminShouldBeAbleToDeleteUserAccountWithoutProvidingPassword()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            await AuthHelper.RunAsAdministratorAsync(_factory);

            await _mediator.Send(new DeleteUserCommand { UserId = userId });

            user = await AuthHelper.GetUserByTokenAsync(_factory, token);

            user.Should().BeNull();
        }
    }
}