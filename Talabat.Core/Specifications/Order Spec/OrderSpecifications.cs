using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string buyerEmail)
      : base(O => O.BuyerEmail == buyerEmail )
        {
            AddOrderIncludes();

            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpecifications(string buyerEmail, int orderId )
            :base(O =>  O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            AddOrderIncludes();

        }

        private void AddOrderIncludes()
        {
            Includes.Add(O => O.deliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}

