using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Commands.DeleteShopOpinion;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Commands
{
    /// <summary>
    /// Delete shop opinion tests
    /// </summary>
    public class DeleteShopOpinionTests : IntegrationTest
    {
        /// <summary>
        /// Delete shop opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteShopOpinionWithIncorrectIdShouldThrowNotFound()
        {
            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteShopOpinionCommand() {ShopOpinionId = Guid.Empty})).Should()
                .Throw<NotFoundException>();
        }

        /// <summary>
        /// Delete shop opinion command should delete shop opinion
        /// </summary>
        [Fact]
        public async Task ShouldDeleteFavourite()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create shop opinion firstly
            var shopOpinionToDeleteDto = await _mediator.Send(new CreateShopOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
            });

            //delete
            await _mediator.Send(new DeleteShopOpinionCommand() {ShopOpinionId = shopOpinionToDeleteDto.Id});

            //Assert that deleted
            var item = await DbHelper.FindAsync<ShopOpinion>(_factory, shopOpinionToDeleteDto.Id);
            item.Should().BeNull();
        }

        /// <summary>
        /// User should not be able to delete other user shop opinion
        /// </summary>
        [Fact]
        public async Task UserShouldNotBeAbleToDeleteOtherUserShopOpinion()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create shop opinion firstly
            var ShopOpinionToDeleteDto = await _mediator.Send(new CreateShopOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
            });

            //change user
            _factory.CurrentUserId = Guid.NewGuid(); 

            //delete
            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteShopOpinionCommand() {ShopOpinionId = ShopOpinionToDeleteDto.Id})).Should()
                .Throw<ForbiddenAccessException>();
        }

        /// <summary>
        /// Administrator should be able to delete user shop opinion
        /// </summary>
        [Fact]
        public async Task AdministratorShouldBeAbleToDeleteUserShopOpinion()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopsAsync(_factory);

            //create shop opinion firstly
            var shopOpinionToDeleteDto = await _mediator.Send(new CreateShopOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
            });

            await AuthHelper
                .RunAsAdministratorAsync(_factory);

            //delete
            await _mediator.Send(new DeleteShopOpinionCommand() {ShopOpinionId = shopOpinionToDeleteDto.Id});

            //Assert that deleted
            var item = await DbHelper.FindAsync<ShopOpinion>(_factory, shopOpinionToDeleteDto.Id);
            item.Should().BeNull();
        }
    }
}