using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.DeleteUser;
using Application.Users.Responses;
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
            var token = result.As<AuthSuccessResponse>().Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            _factory.CurrentUserId = userId;
            _factory.CurrentUserRole = Roles.User;

            await _mediator.Send(new DeleteUserCommand() { UserId = userId });

            user = await AuthHelper.GetUserByTokenAsync(_factory, token);

            user.Should().BeNull();
        }

        [Fact]
        public async Task UserShouldNotBeAbleToDeleteOtherUserAccount()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.As<AuthSuccessResponse>().Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            await AuthHelper.RunAsDefaultUserAsync(_factory);

            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteUserCommand() { UserId = userId }))
                .Should()
                .Throw<ForbiddenAccessException>();
        }

        [Fact]
        public async Task AdminShouldBeAbleToDeleteUserAccount()
        {
            //First, register user
            var result = await AuthHelper.RegisterTestUserAsync(_mediator);
            var token = result.As<AuthSuccessResponse>().Token;
            var user = await AuthHelper.GetUserByTokenAsync(_factory, token);
            var userId = user.Id;

            await AuthHelper.RunAsAdministratorAsync(_factory);

            await _mediator.Send(new DeleteUserCommand() { UserId = userId });

            user = await AuthHelper.GetUserByTokenAsync(_factory, token);

            user.Should().BeNull();
        }
    }
}