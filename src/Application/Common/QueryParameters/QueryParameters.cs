using Application.Common.Enums;

namespace Application.Common.QueryParameters
{
    /// <summary>
    ///     Query parameters
    /// </summary>
    public abstract class QueryParameters
    {
        private const int maxPageSize = 30;
        private int _pageSize = 10;

        /// <summary>
        ///     The column by which to sort
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        ///     The sort direction
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.ASC;

        /// <summary>
        ///     The search query
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        ///     The page number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        ///     The page size
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}