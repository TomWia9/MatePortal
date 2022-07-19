using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateImages.Commands.DeleteYerbaMateImage;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMateImages.Commands;

/// <summary>
///     Delete yerba mate image tests
/// </summary>
public class DeleteYerbaMateImageTests : IntegrationTest
{
    /// <summary>
    ///     Delete image with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteImageWithIncorrectIdShouldThrowNotFound()
    {
        var yerbaMateImageId = Guid.Empty;

        FluentActions.Invoking(() =>
                Mediator.Send(new DeleteYerbaMateImageCommand() {YerbaMateImageId = yerbaMateImageId})).Should()
            .ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete image command should delete image
    /// </summary>
    [Fact]
    public async Task ShouldDeleteImage()
    {
        await TestSeeder.SeedTestYerbaMateImagesAsync(Factory);

        var yerbaMateImageId = Guid.Parse("49B83785-C3B4-4839-8D30-4B5BDF7D85AD"); //one of seeded images

        //delete
        await Mediator.Send(new DeleteYerbaMateImageCommand {YerbaMateImageId = yerbaMateImageId});

        //Assert that deleted
        var item = await DbHelper.FindAsync<YerbaMateImage>(Factory, yerbaMateImageId);
        item.Should().BeNull();
    }
}