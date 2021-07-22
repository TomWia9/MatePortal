using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;
using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class FilterService<T> : IFilterService<T>
        where T : class
    {
        public IQueryable<T> Filter(IQueryable<T> collection, Dictionary<string, object> filterColumns)
        {
            foreach (var (key, value) in filterColumns)
            {
                collection =
                    collection.Where(x => x.GetType().GetProperty(key).GetValue(x, null) == value);
            }

            return collection;
        }

        public IQueryable<T> Search(IQueryable<T> collection, string searchQuery,
            IEnumerable<string> searchColumns)
        {
            searchQuery = searchQuery.Trim();

            foreach (var column in searchColumns)
            {
                collection = collection.Where(x =>
                    x.GetType().GetProperty(column).GetValue(x, null).ToString().Contains(searchQuery));
            }

            return collection;
        }

        public IQueryable<T> Sort(IQueryable<T> collection, string sortBy, SortDirection sortDirection,
            Dictionary<string, Expression<Func<T, object>>> sortingColumnsSelector)
        {
            var selectedColumn = sortingColumnsSelector[sortBy.FirstCharToUpper()];

            collection = sortDirection == SortDirection.ASC
                ? collection.OrderBy(selectedColumn)
                : collection.OrderByDescending(selectedColumn);

            return collection;
        }
    }
}