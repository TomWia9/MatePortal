using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Commands.CreateOpinion;
using Application.Opinions.Commands.DeleteOpinion;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands;

/// <summary>
///     Delete opinion tests
/// </summary>
public class DeleteOpinionTests : IntegrationTest
{
    /// <summary>
    ///     Delete opinion with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteOpinionWithIncorrectIdShouldThrowNotFound()
    {
        var opinionId = Guid.Empty;

        FluentActions.Invoking(() =>
                Mediator.Send(new DeleteOpinionCommand {OpinionId = opinionId})).Should()
            .ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete opinion command should delete opinion
    /// </summary>
    [Fact]
    public async Task ShouldDeleteFavourite()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create opinion firstly
        var opinionToDeleteDto = await Mediator.Send(new CreateOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        //delete
        await Mediator.Send(new DeleteOpinionCommand {OpinionId = opinionToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Opinion>(Factory, opinionToDeleteDto.Id);
        item.Should().BeNull();
    }

    /// <summary>
    ///     User should not be able to delete other user opinion
    /// </summary>
    [Fact]
    public async Task UserShouldNotBeAbleToDeleteOtherUserOpinion()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create opinion firstly
        var opinionToDeleteDto = await Mediator.Send(new CreateOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        Factory.CurrentUserId = Guid.NewGuid(); //other user

        //delete
        await FluentActions.Invoking(() =>
                Mediator.Send(new DeleteOpinionCommand {OpinionId = opinionToDeleteDto.Id})).Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    /// <summary>
    ///     Administrator should be able to delete user opinion
    /// </summary>
    [Fact]
    public async Task AdministratorShouldBeAbleToDeleteUserOpinion()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        //create opinion firstly
        var opinionToDeleteDto = await Mediator.Send(new CreateOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        await AuthHelper
            .RunAsAdministratorAsync(Factory);

        //delete
        await Mediator.Send(new DeleteOpinionCommand {OpinionId = opinionToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Opinion>(Factory, opinionToDeleteDto.Id);
        item.Should().BeNull();
    }

    /// <summary>
    ///     Delete should decrease yerba mate number of opinions
    /// </summary>
    [Fact]
    public async Task ShouldDecreaseYerbaMateNumberOfOpinions()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateOpinionCommand
        {
            Comment = "Test",
            Rate = 8,
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        var opinionToDeleteDto = await Mediator.Send(command);

        //delete
        await Mediator.Send(new DeleteOpinionCommand {OpinionId = opinionToDeleteDto.Id});

        var yerbaMateDto = await Mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
        yerbaMateDto.NumberOfOpinions.Should().Be(0);
    }
}