using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Queries;
using Application.Shops.Queries.GetShop;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Commands
{
    /// <summary>
    ///     Create shop opinion tests
    /// </summary>
    public class CreateOpinionTests : IntegrationTest
    {
        /// <summary>
        ///     Create shop opinion should create opinion and return shop opinion data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldCreateShopOpinionAndReturnShopOpinionDto()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);

            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateShopOpinionCommand
            {
                Rate = 8,
                Comment = "Test comment",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //one of seeded shops
            };

            var result = await _mediator.Send(command);

            var item = await DbHelper.FindAsync<ShopOpinion>(_factory, result.Id);

            result.Should().BeOfType<ShopOpinionDto>();
            result.Rate.Should().Be(command.Rate);
            result.Comment.Should().Be(command.Comment);
            result.Created.Should().BeCloseTo(DateTime.Now, 1000);
            result.ShopId.Should().Be(command.ShopId);
            result.CreatedBy.Should().Be(userId);

            item.CreatedBy.Should().NotBeNull();
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModified.Should().BeNull();
            item.LastModifiedBy.Should().BeNull();
        }


        /// <summary>
        ///     Create shop opinion for nonexistent shop should throw NotFound
        /// </summary>
        [Fact]
        public async Task CreateShopOpinionForNonexistentShopShouldThrowNotFound()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateShopOpinionCommand
            {
                Rate = 2,
                Comment = "test",
                ShopId = Guid.NewGuid()
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        ///     Shop opinion should not be added more than once to one shop by one user
        /// </summary>
        [Fact]
        public async Task ShopOpinionShouldNotBeAddedMoreThanOnceToOneShopByOneUser()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //id of one of seeded shops

            await _mediator.Send(new CreateShopOpinionCommand
            {
                Rate = 10,
                Comment = "Test",
                ShopId = shopId
            });

            var command = new CreateShopOpinionCommand
            {
                Rate = 8,
                Comment = "Test 2",
                ShopId = shopId
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<ConflictException>();
        }

        /// <summary>
        ///     Create should increase shop number of opinions
        /// </summary>
        [Fact]
        public async Task ShouldIncreaseShopNumberOfOpinions()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateShopOpinionCommand
            {
                Comment = "Test",
                Rate = 8,
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //one of seeded shop
            };

            await _mediator.Send(command);

            var shopDto = await _mediator.Send(new GetShopQuery(command.ShopId));
            shopDto.NumberOfShopOpinions.Should().Be(1);
        }
    }
}