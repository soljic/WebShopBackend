using Domain;
using MediatR;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Basket.Queries
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, CustomerBasket>
    {

        private readonly IDatabase _database;
        public GetBasketQueryHandler(IConnectionMultiplexer context)
        {
            this._database = context.GetDatabase();

        }
        public async Task<CustomerBasket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {

            var basket = await _database.StringGetAsync(request.basketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }
    }
}
