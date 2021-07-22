using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;

namespace Application.Common.Interfaces
{
    public interface IFilterService<T>
        where T : class
    {
        IQueryable<T> Filter(IQueryable<T> collection, Dictionary<string, object> filterColumns);

        IQueryable<T> Search(IQueryable<T> collection, string searchQuery, IEnumerable<string> searchColumns);

        IQueryable <T> Sort(IQueryable<T> collection, string sortBy, SortDirection sortDirection,
            Dictionary<string, Expression<Func<T, object>>> sortingColumnsSelector);
    }
}