﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.product.PictureUrl}";
            return string.Empty;
        }
    }
}
