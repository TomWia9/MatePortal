using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Shops.Commands.DeleteShop;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Shops.Commands
{
    /// <summary>
    ///     Delete shop tests
    /// </summary>
    public class DeleteShopTests : IntegrationTest
    {
        /// <summary>
        ///     Delete shop with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteShopWithIncorrectIdShouldThrowNotFound()
        {
            FluentActions.Invoking(() =>
                _mediator.Send(new DeleteShopCommand {ShopId = Guid.Empty})).Should().ThrowAsync<NotFoundException>();
        }

        /// <summary>
        ///     Delete shop command should delete shop
        /// </summary>
        [Fact]
        public async Task ShouldDeleteShop()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);

            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

            //delete
            await _mediator.Send(new DeleteShopCommand {ShopId = shopId});

            //Assert that deleted
            var item = await DbHelper.FindAsync<Shop>(_factory, shopId);
            item.Should().BeNull();
        }

        //Delete cascade is probably not supported in InMemoryDb

        // /// <summary>
        // /// Delete shop should delete all opinions about this shop
        // /// </summary>
        // [Fact]
        // public async Task DeleteShopShouldDeleteAllOpinionsAboutThisShop()
        // {
        //     await TestSeeder.SeedTestShopsAsync(_factory);
        //     await TestSeeder.SeedTestShopOpinionsAsync(_factory);
        //
        //     //one of seeded shop
        //     var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5");
        //
        //     //seeded opinions about this shop
        //     var shopOpinions = new List<Guid>()
        //     {
        //         Guid.Parse("A0EDB43D-5195-4458-8C4B-8F6F9FD7E5C9"),
        //         Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5")
        //     };
        //
        //     //delete
        //     await _mediator.Send(new DeleteShopCommand() { ShopId = shopId });
        //
        //     //Assert that opinions about this shop deleted
        //     foreach (var opinionId in shopOpinions)
        //     {
        //         var item = await DbHelper.FindAsync<ShopOpinion>(_factory, opinionId);
        //         item.Should().BeNull();
        //     }
        // }
    }
}