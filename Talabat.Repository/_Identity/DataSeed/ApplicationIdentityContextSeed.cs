using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository._Identity.DataSeed
{
    public static class ApplicationIdentityContextSeed
    {
        public static async Task UserIdentitySeed(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var User = new ApplicationUser()
                {
                    DisplayName = "youssef samir",
                    Email = "youssef.samir@gmail.com",
                    UserName = "youssef.samir",
                    PhoneNumber = "01202203469"
                };

                // insert user in identityDbcontext [database] -- object Usermanger<ApplicationUser> 

                await userManager.CreateAsync(User , "P@$$w0rd");
            }
        }
    }
}
