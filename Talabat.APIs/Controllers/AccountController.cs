using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract.Auth.contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager 
            , SignInManager<ApplicationUser> signInManager
            ,IAuthService authService
            ,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        // EndPoint Login 
        [HttpPost("login")]  // baseUrl/api/Account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401, "Invaled Login"));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invaled Login"));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user , _userManager)
            });
        }

        // EndPoint for registeration 
        [HttpPost("register")]  // POST : baseUrl/api/Account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var EmailUser = await _userManager.FindByEmailAsync(model.Email);
            if (EmailUser is not null) return BadRequest(new ApiResponse(400, "The Email is Exist"));

            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiValidationResponse() { Errors = result.Errors.Select(E => E.Description) });

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }


        // GET : /api/Account
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurtentUser()
        {
            // get user --> authorize -- token ---> decryption --> claims --> email , name 
            // get user by email 

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }


        // getUserAddress  
        // GET : /api/account/address
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            // user --> token --> decryption --> claims --> email --> user 
            var user = await _userManager.GetUserByAddressAsync(User);

            return Ok(_mapper.Map<AddressDto>(user?.Address));
        }


        [Authorize]
        [HttpPut("updateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var UpdatedAddress = _mapper.Map<Address>(address);

            var user = await _userManager.GetUserByAddressAsync(User);

            UpdatedAddress.Id = user.Address.Id;

            user.Address = UpdatedAddress;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            
            return Ok(address);
        }

    }
}
