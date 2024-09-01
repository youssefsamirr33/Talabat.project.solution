using Talabat.Core.Entities.Order;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public OrderAddress ShippingAddress { get; set; } = null!;

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>(); 

        public string deliveryMethodName { get; set; } = null!;
        public string deliveryMethodCost { get; set; } = null!;
        public decimal subTotal { get; set; }
        public decimal Total { get; set; } // set data from method --> GetTotal
        public string PaymentIntentId { get; set; } 
    }
}
