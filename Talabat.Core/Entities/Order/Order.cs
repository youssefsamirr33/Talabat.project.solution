using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order : BaseEntity
    {
        private Order()
        {

        }
        public Order(string buyerEmail, OrderAddress shippingAddress, ICollection<OrderItem> items, DeliveryMethod? deliveryMethod, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            this.deliveryMethod = deliveryMethod;
            this.subTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational property [many]

        public DeliveryMethod? deliveryMethod { get; set; } = null!; // Navigational property [ONE]
        public decimal subTotal { get; set; }
        public decimal GetTotal => subTotal + deliveryMethod.Cost;
        public string PaymentIntentId { get; set; } 
    }
}
