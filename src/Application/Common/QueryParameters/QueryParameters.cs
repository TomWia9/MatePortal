using Application.Common.Enums;

namespace Application.Common.QueryParameters
{
    public abstract class QueryParameters
    {
        private const int maxPageSize = 30;
        private int _pageSize = 10;
        
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.ASC;
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
    }
}