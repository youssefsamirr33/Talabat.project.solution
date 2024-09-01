using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Basket;

namespace Talabat.APIs.DTOs
{
    public class CustmorBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public List<BasketItemsDto> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        public decimal? ShippingPrice { get; set; }
    }
}
