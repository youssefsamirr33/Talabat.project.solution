using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.Helpers
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>().ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                                                    .ForMember(d => d.Category , o => o.MapFrom(s => s.Category.Name))
                                                    .ForMember(d => d.PictureUrl , o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustmorBasketDto, CustmorBasket>();
            CreateMap<BasketItemsDto, BasketItems>();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderAddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.deliveryMethodName , o => o.MapFrom(s => s.deliveryMethod.ShortName))
                .ForMember(d => d.deliveryMethodCost , o => o.MapFrom(s => s.deliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId , o => o.MapFrom(s => s.product.ProductId))
                .ForMember(d => d.ProductName , o => o.MapFrom(s => s.product.ProductName))
                .ForMember(d => d.PictureUrl , o => o.MapFrom(s => s.product.PictureUrl))
                .ForMember(d => d.PictureUrl , o => o.MapFrom<OrderItemPictureUrlResolver>());


        }
    }
}
