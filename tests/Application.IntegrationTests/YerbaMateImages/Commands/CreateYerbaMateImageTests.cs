using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateImages.Commands.CreateYerbaMateImage;
using Application.YerbaMateImages.Queries;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.YerbaMateImages.Commands;

/// <summary>
///     Create yerba mate image tests
/// </summary>
public class CreateYerbaMateImageTests : IntegrationTest
{
    /// <summary>
    ///     Create yerba mate image should create yerba mate image and return yerba mate image data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateYerbaMateImageAndReturnYerbaMateImageDto()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateYerbaMateImageCommand()
        {
            Url = "https://images.unsplash.com/photo-1609541994821-d909982e2f1b?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=687&q=80",
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mates
        };

        var result = await Mediator.Send(command);

        var item = await DbHelper.FindAsync<YerbaMateImage>(Factory, result.Id);

        result.Should().BeOfType<YerbaMateImageDto>();
        result.Url.Should().Be(command.Url);
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
    ///     Create image for nonexistent yerba mate should throw NotFound
    /// </summary>
    [Fact]
    public async Task CreateImageForNonexistentYerbaMateShouldThrowNotFound()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateYerbaMateImageCommand()
        {
            Url = "https://images.unsplash.com/photo-1609541994821-d909982e2f1b?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=687&q=80",
            YerbaMateId = Guid.NewGuid()
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }
}