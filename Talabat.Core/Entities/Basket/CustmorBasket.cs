using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket
{
    public class CustmorBasket
    {
        public string Id { get; set; } = null!;
        public List<BasketItems> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        public decimal? ShippingPrice { get; set; }

        public CustmorBasket(string id)
        {
            Id = id;
            Items = new List<BasketItems>();
        }
    }
}
