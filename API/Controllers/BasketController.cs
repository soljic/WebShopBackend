using API.DTOs;
using Application.Basket.Command.CreateAndUpdate;
using Application.Basket.Command.Delete;
using Application.Basket.Queries;
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IMediator _mediatR;
        private readonly IMapper _mapper;

        public BasketController(IMediator mediatR, IMapper mapper)
        {
            this._mediatR = mediatR;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasket(string id)
        {
            if (id == null) throw new Exception("Id cannot be null!");
            var result = await _mediatR.Send(new GetBasketQuery(id));

            if (result == null) throw new Exception("Problem geting customer basket!");
            var customerBasket = _mapper.Map<CustomerBasketDto>(result);

            return customerBasket;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateBasket([FromBody] CustomerBasketDto customerBasketDto)
        {
            if (customerBasketDto == null) throw new Exception("CustomerBasket cannot be null!");

            var customerBasket = _mapper.Map<CustomerBasket>(customerBasketDto);

            var result = await _mediatR.Send(new CreateBasketCommand(customerBasket));

            if (result == null) throw new Exception("Problem creating customer basket!");

            return result;
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasketDto customerBasketDto)
        {
            if (customerBasketDto == null) throw new Exception("CustomerBasket cannot be null!");

            var customerBasket = _mapper.Map<CustomerBasket>(customerBasketDto);

            var result = await _mediatR.Send(new CreateBasketCommand(customerBasket));

            if (result == null) throw new Exception("Problem updating customer basket!");

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<Result<Unit>> DeleteBasket(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("Id cannot be null!");

            var result = await _mediatR.Send(new DeleteBasketCommand(id));

            return result;
        }
    }
}
