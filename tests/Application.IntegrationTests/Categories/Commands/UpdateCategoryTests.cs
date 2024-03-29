﻿using System;
using System.Threading.Tasks;
using Application.Categories.Commands.CreateCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Categories.Commands;

/// <summary>
///     Update category tests
/// </summary>
public class UpdateCategoryTests : IntegrationTest
{
    /// <summary>
    ///     Update category with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateCategoryWithIncorrectIdShouldThrowNotFound()
    {
        var categoryId = Guid.Empty;

        var command = new UpdateCategoryCommand
        {
            CategoryId = categoryId,
            Name = "updated name",
            Description = "updated description"
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update category command should update category
    /// </summary>
    [Fact]
    public async Task UpdateCategoryShouldUpdateCategory()
    {
        await TestSeeder.SeedTestCategoriesAsync(Factory);

        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);
        var categoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"); //one of seeded category

        var command = new UpdateCategoryCommand
        {
            CategoryId = categoryId,
            Name = "updated name",
            Description = "updated description"
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<Category>(Factory, categoryId);

        item.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
    }
    
    /// <summary>
    ///     Category update should require unique name
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueNameWhenUpdating()
    {
        await TestSeeder.SeedTestCategoriesAsync(Factory);

        var command = new UpdateCategoryCommand()
        {
            CategoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"), //one of seeded categories
            Name = "Fruit", //already exists
            Description = "Test"
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
}