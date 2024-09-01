using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data.Config.Order_Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(Order => Order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(order => order.Status).HasConversion((status) => status.ToString() , (status) => (OrderStatus)Enum.Parse(typeof(OrderStatus) , status));

            builder.HasOne(order => order.deliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.Property(order => order.subTotal).HasColumnType("decimal(12,2)");
        }
    }
}
