using Application.Common.QueryParameters;

namespace Application.Opinions.Queries
{
    public class OpinionsQueryParameters : QueryParameters
    {
        private int _minRate = 1;
        private int _maxRate = 10;

        public int MinRate
        {
            get => _minRate;
            set => _minRate = value is < 1 or > 10 ? 1 : value;
        }

        public int MaxRate
        {
            get => _maxRate;
            set => _maxRate = value is < 1 or > 10 ? 10 : value;
        }
    }
}