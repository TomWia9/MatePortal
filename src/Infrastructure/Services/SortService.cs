using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;
using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class SortService<T> : ISortService<T>
        where T : class
    {
        public IQueryable<T> Sort(IQueryable<T> collection, string sortBy, SortDirection sortDirection,
            Dictionary<string, Expression<Func<T, object>>> sortingColumns)
        {
            var selectedColumn = sortingColumns[sortBy.FirstCharToUpper()];

            collection = sortDirection == SortDirection.ASC
                ? collection.OrderBy(selectedColumn)
                : collection.OrderByDescending(selectedColumn);

            return collection;
        }
    }
}