using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mappings
{
    /// <summary>
    ///     Mapping extensions
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        ///     Creates paginated list
        /// </summary>
        /// <param name="queryable">The queryable</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="pageSize">The page size</param>
        /// <typeparam name="TDestination">The type of destination object</typeparam>
        /// <returns>PaginatedList of specified type</returns>
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
        {
            return PaginatedList<TDestination>.ToPaginatedListAsync(queryable, pageNumber, pageSize);
        }

        /// <summary>
        ///     Projects queryable to list
        /// </summary>
        /// <param name="queryable">The queryable</param>
        /// <param name="configuration">The configuration</param>
        /// <typeparam name="TDestination">The type of destination object</typeparam>
        /// <returns>List of specified type</returns>
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable,
            IConfigurationProvider configuration)
        {
            return queryable.ProjectTo<TDestination>(configuration).ToListAsync();
        }
    }
}