using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo , IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }


        // Get basket or ReCreate Basket
        [HttpGet]
        public async Task<ActionResult<CustmorBasket>> GetBasket(string basketId)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            return basket is null ? new CustmorBasket(basketId) : Ok(basket);
        }

        // Update basket or create new basket 
        [HttpPost]
        public async Task<ActionResult<CustmorBasket>> UpdateBasket(CustmorBasketDto basket)
        {
            var basketMapping = _mapper.Map<CustmorBasketDto , CustmorBasket>(basket);
            var CreateOrUpdateBasket = await _basketRepo.UpdateBasketAsync(basketMapping);
            if (CreateOrUpdateBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreateOrUpdateBasket);
        }

        // delete basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            return await _basketRepo.DeleteBasketAsync(basketId);
        }
    }
}
