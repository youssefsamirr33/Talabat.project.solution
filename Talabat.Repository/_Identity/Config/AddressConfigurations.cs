using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository._Identity.Config
{
    internal class AddressConfigurations /*: IEntityTypeConfiguration<Address>*/
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(d => d.LastName).IsRequired().HasMaxLength(50);

            builder.Property(d => d.Street).IsRequired().HasMaxLength(100);
            builder.Property(d => d.City).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Country).IsRequired().HasMaxLength(100);

        }
    }
}
