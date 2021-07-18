using System;
using MediatR;

namespace Application.ShopOpinions.Commands.UpdateShopOpinion
{
    public class UpdateShopOpinionCommand : IRequest
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public Guid ShopId { get; set; }
        public Guid UserId { get; set; }
    }
}