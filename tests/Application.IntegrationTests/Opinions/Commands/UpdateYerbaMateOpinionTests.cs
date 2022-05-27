using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;
using Application.YerbaMateOpinions.Commands.UpdateYerbaMateOpinion;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands;

/// <summary>
///     Update yerba mate opinion tests
/// </summary>
public class UpdateYerbaMateOpinionTests : IntegrationTest
{
    /// <summary>
    ///     Update opinion with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateOpinionWithIncorrectIdShouldThrowNotFound()
    {
        var opinionId = Guid.Empty;

        var command = new UpdateYerbaMateOpinionCommand
        {
            OpinionId = opinionId,
            Rate = 2,
            Comment = "Updated comment"
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update opinion command should update opinion
    /// </summary>
    [Fact]
    public async Task UpdateOpinionShouldUpdateOpinion()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var opinionId = Guid.Parse("EB2BB300-A4FF-486C-AB64-4EF0A7DB527F"); //one of seeded opinions

        var command = new UpdateYerbaMateOpinionCommand
        {
            OpinionId = opinionId,
            Rate = 4,
            Comment = "Updated comment"
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<YerbaMateOpinion>(Factory, opinionId);

        item.Rate.Should().Be(command.Rate);
        item.Comment.Should().Be(command.Comment);
        item.CreatedBy.Should().NotBeNull();
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
    }

    /// <summary>
    ///     User should not be able to update other user opinion
    /// </summary>
    [Fact]
    public async Task UserShouldNotBeAbleToUpdateOtherUserOpinion()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create opinion firstly
        var opinionToUpdateDto = await Mediator.Send(new CreateYerbaMateOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        Factory.CurrentUserId = Guid.NewGuid(); //other user

        await FluentActions.Invoking(() =>
                Mediator.Send(new UpdateYerbaMateOpinionCommand
                    {OpinionId = opinionToUpdateDto.Id, Comment = "test", Rate = 1})).Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    /// <summary>
    ///     Administrator should be able to update user opinion
    /// </summary>
    [Fact]
    public async Task AdministratorShouldBeAbleToUpdateUserOpinion()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        //create opinion firstly
        var opinionToUpdateDto = await Mediator.Send(new CreateYerbaMateOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        await AuthHelper.RunAsAdministratorAsync(Factory);

        var command = new UpdateYerbaMateOpinionCommand
        {
            OpinionId = opinionToUpdateDto.Id,
            Comment = "test1",
            Rate = 2
        };

        await Mediator.Send(command);

        //Assert that updated
        var item = await DbHelper.FindAsync<YerbaMateOpinion>(Factory, opinionToUpdateDto.Id);
        item.Rate.Should().Be(command.Rate);
        item.Comment.Should().Be(command.Comment);
        item.CreatedBy.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModifiedBy.Should().NotBeNull();
    }
}