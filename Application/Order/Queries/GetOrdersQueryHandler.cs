using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Queries
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderDto>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IUserAccessor userAccessor, DataContext context, IMapper mapper)
        {
            _userAccessor = userAccessor;
            _context = context;
            _mapper = mapper;
        }


        public async Task<IReadOnlyList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new Exception("Email cannot be null!");

            var orders = await _context.Orders.Include(x => x.OrderItems).Include(x => x.DeliveryMethod).Where(x => x.BuyerEmail == request.email).OrderByDescending(x => x.OrderDate).ToListAsync();
            if(orders == null)
                throw new Exception("Problem getting orders");
            var ordersDto = _mapper.Map<IReadOnlyList<OrderDto>>(orders);
            return ordersDto;
        }
    }
}
