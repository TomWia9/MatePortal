using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Common.Enums;

namespace Application.Common.Interfaces
{
    public interface ISortService<T>
        where T : class
    {
        IQueryable <T> Sort(IQueryable<T> collection, string sortBy, SortDirection sortDirection,
            Dictionary<string, Expression<Func<T, object>>> sortingColumns);
    }
}