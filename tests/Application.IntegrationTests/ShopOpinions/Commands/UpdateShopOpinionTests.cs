using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Commands.UpdateShopOpinion;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Commands
{
    /// <summary>
    /// Update shop opinion tests
    /// </summary>
    public class UpdateShopOpinionTests : IntegrationTest
    {
        /// <summary>
        /// Update shop opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void UpdateShopOpinionWithIncorrectIdShouldThrowNotFound()
        {
            var command = new UpdateShopOpinionCommand()
            {
                ShopId = Guid.Empty,
                Rate = 2,
                Comment = "Updated comment",
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Update shop opinion command should update shop opinion
        /// </summary>
        [Fact]
        public async Task UpdateShopOpinionShouldUpdateShopOpinion()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);

            var shopOpinionId = Guid.Parse("A0EDB43D-5195-4458-8C4B-8F6F9FD7E5C9"); //one of seeded shop opinions

            var command = new UpdateShopOpinionCommand()
            {
                ShopOpinionId = shopOpinionId,
                Rate = 4,
                Comment = "Updated comment"
            };

            await _mediator.Send(command);

            var item = await DbHelper.FindAsync<ShopOpinion>(_factory, shopOpinionId);

            item.Rate.Should().Be(command.Rate);
            item.Comment.Should().Be(command.Comment);
            item.CreatedBy.Should().NotBeNull();
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
        }

        /// <summary>
        /// User should not be able to update other user shop opinion
        /// </summary>
        [Fact]
        public async Task UserShouldNotBeAbleToUpdateOtherUserShopOpinion()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create shop opinion firstly
            var shopOpinionToUpdateDto = await _mediator.Send(new CreateShopOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
            });

            //change user
            _factory.CurrentUserId = Guid.NewGuid();

            FluentActions.Invoking(() =>
                    _mediator.Send(new UpdateShopOpinionCommand()
                        {ShopOpinionId = shopOpinionToUpdateDto.Id, Comment = "test", Rate = 1})).Should()
                .Throw<ForbiddenAccessException>();
        }

        /// <summary>
        /// Administrator should be able to update user shop opinion
        /// </summary>
        [Fact]
        public async Task AdministratorShouldBeAbleToUpdateUserShopOpinion()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopsAsync(_factory);

            //create shop opinion firstly
            var shopOpinionToUpdateDto = await _mediator.Send(new CreateShopOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
            });

            //change user
            var adminId = await AuthHelper.RunAsAdministratorAsync(_factory);

            var command = new UpdateShopOpinionCommand()
            {
                ShopOpinionId = shopOpinionToUpdateDto.Id,
                Comment = "test1",
                Rate = 2
            };

            await _mediator.Send(command);

            //Assert that updated
            var item = await DbHelper.FindAsync<ShopOpinion>(_factory, shopOpinionToUpdateDto.Id);
            item.Rate.Should().Be(command.Rate);
            item.Comment.Should().Be(command.Comment);
            item.CreatedBy.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModifiedBy.Should().NotBeNull();
        }
    }
}