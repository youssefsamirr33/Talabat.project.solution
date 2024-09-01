using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.DTOs
{
    public class OrderDto
    {
      
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public string BasketId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; }
    }
}
