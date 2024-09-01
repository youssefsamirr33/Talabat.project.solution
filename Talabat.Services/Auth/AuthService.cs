using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract.Auth.contract;

namespace Talabat.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // generate token --> 1. header --> key - security algorithm
            //                    2. payload  -->  1. private claim
            //                                     2. register claim
            //                    3. value 

            // private claim 
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var userRole = await userManager.GetRolesAsync(user);
            foreach (var Role in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, Role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]));

            var token = new JwtSecurityToken(
                audience: _configuration["JWT:Aud"],
                issuer : _configuration["JWT:Validissuer"],
                expires : DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDay"])),
                claims : authClaims,
                signingCredentials : new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha384Signature)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
