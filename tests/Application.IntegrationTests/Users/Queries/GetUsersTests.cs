using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Users.Commands.RegisterUser;
using Application.Users.Queries;
using Application.Users.Queries.GetUsers;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Users.Queries
{
    /// <summary>
    /// Get all users tests
    /// </summary>
    public class GetUsersTests : IntegrationTest
    {
        /// <summary>
        /// Get users should return all users
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllUsers()
        {
            await AuthHelper.RegisterTestUserAsync(_mediator);

            var command = new RegisterUserCommand()
            {
                Email = "user@test.com",
                Username = "username",
                Password = "Test123_"
            };

            await _mediator.Send(command);

            var response =
                await _mediator.Send(new GetUsersQuery(new UsersQueryParameters()));
            response.Count.Should().Be(2);
        }

        /// <summary>
        /// Get users with specified search query should return correct users
        /// </summary>
        [Fact]
        public async Task GetUsersWithSpecifiedSearchQueryShouldReturnCorrectUsers()
        {
            await AuthHelper.RegisterTestUserAsync(_mediator);

            var command = new RegisterUserCommand()
            {
                Email = "user@test.com",
                Username = "username",
                Password = "Test123_"
            };

            await _mediator.Send(command);

            var response = await _mediator.Send(new GetUsersQuery(new UsersQueryParameters()
            {
                SearchQuery = "ser"
            }));

            response.Count.Should().Be(1);
            response[0].Email.Should().Be("user@test.com");
            response[0].Username.Should().Be("username");
        }
    }
}