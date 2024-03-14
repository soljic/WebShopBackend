using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Basket.Queries
{
    public class GetBasketQuery : IRequest<CustomerBasket>
    {
        public GetBasketQuery(string Id)
        {
            this.basketId = Id;
        }
        public string basketId { get; set; }
    }
}
