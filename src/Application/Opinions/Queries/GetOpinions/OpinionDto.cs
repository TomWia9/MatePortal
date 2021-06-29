using System;

namespace Application.Opinions.Queries.GetOpinions
{
    public class OpinionDto
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid YerbaMateId { get; set; }
        public string User { get; set; }
    }
}