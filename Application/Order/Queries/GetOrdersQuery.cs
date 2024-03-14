using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Queries
{
    public record GetOrdersQuery(string email) : IRequest<IReadOnlyList<OrderDto>>
    {
    }
   
}
