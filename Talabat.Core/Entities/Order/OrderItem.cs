using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        private OrderItem()
        {
            
        }
        public OrderItem(OrderProduct product, decimal price, int quantity)
        {
            this.product = product;
            Price = price;
            Quantity = quantity;
        }

        public OrderProduct product { get; set; } = null!;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
