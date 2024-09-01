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
    internal class ApplicationUserConfigurations/* : IEntityTypeConfiguration<ApplicationUser>*/
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.DisplayName).IsRequired();
            builder.HasOne(d => d.Address).WithOne(u => u.User);
        }
    }
}
