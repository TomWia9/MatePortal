﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Users.Queries;
using Application.Users.Queries.GetUser;
using FluentAssertions;
using Infrastructure.Identity;
using Xunit;

namespace Application.IntegrationTests.Users.Queries;

/// <summary>
///     Get user tests
/// </summary>
public class GetUserTests : IntegrationTest
{
    /// <summary>
    ///     Get user should return user
    /// </summary>
    [Fact]
    public async Task ShouldReturnUser()
    {
        //First, register user
        var result = await AuthHelper.RegisterTestUserAsync(Mediator);
        var token = result.Token;
        var user = await AuthHelper.GetUserByTokenAsync(Factory, token);
        var userId = user.Id;

        var response =
            await Mediator.Send(new GetUserQuery(userId));

        response.Should().BeOfType<UserDto>();
        response.Email.Should().Be(user.Email);
        response.Username.Should().Be(user.UserName);
        response.Role.Should().Be(Roles.User);
    }

    /// <summary>
    ///     Get user with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void GetUserWithIncorrectIdShouldThrowNotFound()
    {
        FluentActions.Invoking(() =>
                Mediator.Send(new GetUserQuery(Guid.Empty)))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}