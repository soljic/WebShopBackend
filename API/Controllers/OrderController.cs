using Application.Dtos;
using Application.Interfaces;
using Application.Order.Command;
using Application.Order.Queries;
using AutoMapper;
using Domain;
using Domain.OrderAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IMediator _mediator;
         private readonly IUserAccessor _userAccessor;
        private readonly IMapper  _mapper;

        public OrderController(IMediator mediator, IUserAccessor userAccessor, IMapper mapper)
        {
            _mediator = mediator;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(Application.Dtos.OrderDto order)
        {
            var email = _userAccessor.GetEmail();
           //var address = _mapper.Map<OrderAddress,AddressDto>(order.ShippingAddress);
            return Ok(await Mediator.Send(new CreateOrderCommand(order.DeliveryMethodId, email, order.BasketId, order.ShippingAddress)));
        } 
        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders()
        {
            var email = _userAccessor.GetEmail();
            return  Ok(await Mediator.Send(new GetOrdersQuery(email)));
        }

    }
}
