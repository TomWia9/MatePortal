using Application.Common.QueryParameters;

namespace Application.ShopOpinions.Queries
{
    /// <summary>
    ///     Shop opinions query parameters
    /// </summary>
    public class ShopOpinionsQueryParameters : QueryParameters
    {
        private int _maxRate = 10;
        private int _minRate = 1;

        /// <summary>
        ///     Minimum rate parameter
        /// </summary>
        public int MinRate
        {
            get => _minRate;
            set => _minRate = value is < 1 or > 10 ? 1 : value;
        }

        /// <summary>
        ///     Maximum rate parameter
        /// </summary>
        public int MaxRate
        {
            get => _maxRate;
            set => _maxRate = value is < 1 or > 10 ? 10 : value;
        }
    }
}