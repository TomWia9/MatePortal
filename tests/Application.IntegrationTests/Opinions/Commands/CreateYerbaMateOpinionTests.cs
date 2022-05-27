using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;
using Application.YerbaMateOpinions.Queries;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands;

/// <summary>
///     Create yerba mate opinion tests
/// </summary>
public class CreateOpinionTests : IntegrationTest
{
    /// <summary>
    ///     Create yerba mate opinion should create opinion and return yerba mate opinion data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateYerbaMateOpinionAndReturnYerbaMateOpinionDto()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateYerbaMateOpinionCommand
        {
            Rate = 8,
            Comment = "Test comment",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        var result = await Mediator.Send(command);

        var item = await DbHelper.FindAsync<YerbaMateOpinion>(Factory, result.Id);

        result.Should().BeOfType<YerbaMateOpinionDto>();
        result.Rate.Should().Be(command.Rate);
        result.Comment.Should().Be(command.Comment);
        result.Created.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        result.YerbaMateId.Should().Be(command.YerbaMateId);
        result.CreatedBy.Should().Be(userId);

        item.CreatedBy.Should().NotBeNull();
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModified.Should().BeNull();
        item.LastModifiedBy.Should().BeNull();
    }


    /// <summary>
    ///     Create opinion for nonexistent yerba should throw NotFound
    /// </summary>
    [Fact]
    public async Task CreateOpinionForNonexistentYerbaMateShouldThrowNotFound()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        var yerbaMateId = Guid.NewGuid();

        var command = new CreateYerbaMateOpinionCommand
        {
            Rate = 2,
            Comment = "test",
            YerbaMateId = yerbaMateId
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Opinion should not be added more than once to one yerba by one user
    /// </summary>
    [Fact]
    public async Task OpinionShouldNotBeAddedMoreThanOnceToOneYerbaByOneUser()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //id of one of seeded yerba mate

        await Mediator.Send(new CreateYerbaMateOpinionCommand
        {
            Rate = 10,
            Comment = "Test",
            YerbaMateId = yerbaMateId
        });

        var command = new CreateYerbaMateOpinionCommand
        {
            Rate = 8,
            Comment = "Test 2",
            YerbaMateId = yerbaMateId
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }

    /// <summary>
    ///     Create should increase yerba mate number of opinions
    /// </summary>
    [Fact]
    public async Task ShouldIncreaseYerbaMateNumberOfOpinions()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateYerbaMateOpinionCommand
        {
            Comment = "Test",
            Rate = 8,
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        await Mediator.Send(command);


        var yerbaMateDto = await Mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
        yerbaMateDto.NumberOfOpinions.Should().Be(1);
    }
}