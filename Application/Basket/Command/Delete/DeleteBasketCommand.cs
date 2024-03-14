using Application.Core;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Basket.Command.Delete
{
    public class DeleteBasketCommand :IRequest<Result<Unit>>
    {
        public DeleteBasketCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
