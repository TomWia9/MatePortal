using System;
using MediatR;

namespace Application.ShopOpinions.Commands.DeleteShopOpinion
{
    public class DeleteShopOpinionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}