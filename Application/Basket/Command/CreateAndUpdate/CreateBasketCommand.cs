using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Basket.Command.CreateAndUpdate
{
    public class CreateBasketCommand : IRequest<CustomerBasket>
    {
        public CreateBasketCommand()
        {

        }
        public CreateBasketCommand(CustomerBasket customerBasket)
        {

            this.CustomerBasket = customerBasket;

        }
        public CustomerBasket CustomerBasket { get; set; }
    }
}
