using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Basket.Command.CreateAndUpdate
{
    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, CustomerBasket>
    {
        private readonly StackExchange.Redis.IDatabase _database;
        public CreateBasketCommandHandler(IConnectionMultiplexer context)
        {
            this._database = context.GetDatabase();

        }
        public async Task<CustomerBasket> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            if (request.CustomerBasket == null) throw new Exception("Basket is null!");

            var created = await _database.StringSetAsync(request.CustomerBasket.Id, JsonSerializer.Serialize(request.CustomerBasket), TimeSpan.FromDays(30));

            if (!created) throw new Exception("Problem creating a basket!");

            return request.CustomerBasket;

        }
    }
}
