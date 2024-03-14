using Application.Core;
using MediatR;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Basket.Command.Delete
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Result<Unit>>
    {
        private readonly StackExchange.Redis.IDatabase _database;

        public DeleteBasketCommandHandler(IConnectionMultiplexer context)
        {
            _database = context.GetDatabase();
        }

        public async Task<Result<Unit>> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == null) throw new Exception("Basket Id is null!");

            var basket = await  _database.KeyDeleteAsync(request.Id);

            if (!basket) return Result<Unit>.Failure("Failed to delete activity");

            return  Result<Unit>.Success(Unit.Value);
        }
    }
}
