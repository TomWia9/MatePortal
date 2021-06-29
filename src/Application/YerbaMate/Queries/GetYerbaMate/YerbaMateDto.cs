﻿using System;

namespace Application.YerbaMate.Queries.GetYerbaMate
{
    public class YerbaMateDto
    {
        public Guid Id { get; init; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string imgUrl { get; set; }
        public decimal AveragePrice { get; set; }
        public int NumberOfAddToFav { get; set; }
    }
}