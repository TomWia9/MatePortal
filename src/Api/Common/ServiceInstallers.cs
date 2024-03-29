﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Api.Services;
using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Identity;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Common;

/// <summary>
///     The service installers
/// </summary>
public static class ServiceInstallers
{
    /// <summary>
    ///     Adds swagger support
    /// </summary>
    /// <param name="services">The services collection</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.SwaggerDoc(
                "MatePortalSpecification",
                new OpenApiInfo
                {
                    Title = "MatePortal",
                    Version = "1",
                    Contact = new OpenApiContact
                    {
                        Email = "tomaszwiatrowski9@gmail.com",
                        Name = "Tomasz Wiatrowski",
                        Url = new Uri("https://www.linkedin.com/in/tomasz-wiatrowski-279b00176/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

            OpenApiSecurityScheme securityDefinition = new()
            {
                Name = "Bearer",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            };
            setupAction.AddSecurityDefinition("jwt_auth", securityDefinition);

            OpenApiSecurityScheme securityScheme = new()
            {
                Reference = new OpenApiReference
                {
                    Id = "jwt_auth",
                    Type = ReferenceType.SecurityScheme
                }
            };
            OpenApiSecurityRequirement securityRequirements = new()
            {
                {securityScheme, Array.Empty<string>()}
            };
            setupAction.AddSecurityRequirement(securityRequirements);

            //Collect all referenced projects output XML document file paths  
            var currentAssembly = Assembly.GetExecutingAssembly();
            var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new[] {currentAssembly.GetName()})
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location) ?? string.Empty,
                    $"{a.Name}.xml"))
                .Where(File.Exists).ToList();

            foreach (var d in xmlDocs) setupAction.IncludeXmlComments(d);
        });
    }

    /// <summary>
    ///     Adds json web token authorization
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration</param>
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(jwtSettings);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.SaveToken = true;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.UserAccess,
                policy => policy.RequireAssertion(context =>
                    context.User.IsInRole(Roles.User) || context.User.IsInRole(Roles.Administrator)));

            options.AddPolicy(Policies.AdminAccess,
                policy => policy.RequireAssertion(context => context.User.IsInRole(Roles.Administrator)));
        });
    }

    /// <summary>
    ///     Adds internal services
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration</param>
    public static void AddInternalServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddFluentValidationRulesToSwagger();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
    }
}