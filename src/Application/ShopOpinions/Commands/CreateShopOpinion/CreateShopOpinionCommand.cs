using System;
using Application.ShopOpinions.Queries;
using MediatR;

namespace Application.ShopOpinions.Commands.CreateShopOpinion
{
    public class CreateShopOpinionCommand : IRequest<ShopOpinionDto>
    {
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }   
    }
}