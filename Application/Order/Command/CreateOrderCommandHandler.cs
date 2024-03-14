using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.OrderAggregate;
using MediatR;
using Persistence;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Order.Command
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {

        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IDatabase _database;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IUserAccessor userAccessor, DataContext context, IConnectionMultiplexer contextBasket, IMapper mapper)
        {
            _userAccessor = userAccessor;
            _database = contextBasket.GetDatabase();
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var basketRedis = _database.StringGetAsync(request.BasketId).Result;

           var basket = basketRedis.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basketRedis);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            var deliveryMethod = await _context.DeliveryMethods.FindAsync(request.DeliveryMethodId);

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            var order = new Domain.OrderAggregate.Order(orderItems, request.BuyerEmail,request.ShippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);

            if(order == null) throw new Exception("Problem creating order. order is null");
            _context.Orders.Add(order);
            _context.SaveChanges();
            var orderDto = _mapper.Map<OrderDto>(order);
            if(orderDto == null) throw new Exception("Problem creating order, problem mapping orderDto");
            return orderDto;
        }
    }
}
