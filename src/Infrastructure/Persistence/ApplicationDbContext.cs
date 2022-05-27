using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
///     Application database context
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>,
    IApplicationDbContext
{
    /// <summary>
    ///     The current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The date time service
    /// </summary>
    private readonly IDateTime _dateTime;

    /// <summary>
    ///     The domain event service
    /// </summary>
    private readonly IDomainEventService _domainEventService;

    /// <summary>
    ///     Initializes ApplicationDbContext
    /// </summary>
    /// <param name="options">The options</param>
    /// <param name="currentUserService">The current user service</param>
    /// <param name="dateTime">The date time service</param>
    /// <param name="domainEventService">The domain event service</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService, IDateTime dateTime,
        IDomainEventService domainEventService) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
        _domainEventService = domainEventService;
    }

    /// <summary>
    ///     The brands DbSet
    /// </summary>
    public DbSet<Brand> Brands { get; set; }

    /// <summary>
    ///     The Categories DbSet
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    ///     The Countries DbSet
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    ///     User's yerba mate favorites DbSet
    /// </summary>
    public DbSet<Favourite> Favourites { get; set; }

    /// <summary>
    ///     User's yerba mate opinions DbSet
    /// </summary>
    public DbSet<YerbaMateOpinion> YerbaMateOpinions { get; set; }

    /// <summary>
    ///     The YerbaMates DbSet
    /// </summary>
    public DbSet<YerbaMate> YerbaMate { get; set; }

    /// <summary>
    ///     The Shops DbSet
    /// </summary>
    public DbSet<Shop> Shops { get; set; }

    /// <summary>
    ///     User's shop opinions DbSet
    /// </summary>
    public DbSet<ShopOpinion> ShopOpinions { get; set; }

    /// <summary>
    ///     Saves changes async
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The task result of int</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;
            }

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private async Task DispatchEvents()
    {
        while (true)
        {
            var domainEventEntity = ChangeTracker
                .Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .FirstOrDefault(domainEvent => !domainEvent.IsPublished);
            if (domainEventEntity == null) break;

            domainEventEntity.IsPublished = true;
            await _domainEventService.Publish(domainEventEntity);
        }
    }
}