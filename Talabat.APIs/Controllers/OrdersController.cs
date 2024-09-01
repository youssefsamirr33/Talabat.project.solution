using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order;
using Talabat.Core.Repositories.Contract.Order_contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // EndPoint --> POST : /api/orders
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var OrderAddressMapper = _mapper.Map<OrderAddressDto, OrderAddress>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, OrderAddressMapper, orderDto.BasketId , orderDto.DeliveryMethodId);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order , OrderToReturnDto>(order));
        }

        // endpoint --> get orders for spcific user
        [Authorize]
        [HttpGet] // /api/orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<OrderToReturnDto>(orders));

        }

        // endpoint --> get order by id for spcific User 
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(email, id);
            if (order is null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        // endpoint --> get all delivery methods
        [Authorize]
        [HttpGet("deliverymethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
