﻿using System.Security.Claims;
using API.Dtos;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, TokenService tokenService, DataContext context) : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;
        private readonly DataContext _context = context;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                 .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                return await CreateUserObject(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                Fullname = registerDto.Fullname,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return await CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return await CreateUserObject(user);
        }

        //[Authorize]
        //[HttpPost("ChangePassword")]
        //public async Task<ActionResult<Result<Unit>>> ChangePassword(ChangeUserPassword.ChangeUserPasswordCommand command)
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    command.Email = email;
        //    return HandleResult(await Mediator.Send(command));
        //}

        [Authorize]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive) return Unauthorized();

            return await CreateUserObject(user);
        }

        private async Task<UserDto> CreateUserObject(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
          

            return new UserDto
            {
                FullName = user.Fullname,
                AccessToken = _tokenService.CreateToken(user, userClaims, roles),
                Username = user.UserName,
                OrderId = user.Orders.FirstOrDefault(x => !x.Confirmed)?.OrderId,
                Roles = GetRoles(roles),
            };
        }

        private List<string> GetRoles(IList<string> roles)
        {
            List<string> output = [];
            foreach (var role in roles)
            {
                output.Add(role);
            }
            return output;
        }

        private async Task SetRefreshToken(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        private async Task<Order> RetrieveOrder(string orderId)
        {
            //if (string.IsNullOrEmpty(buyerId))
            //{
            //    Response.Cookies.Delete("buyerId");
            //    return null;
            //}

            return await _context.Orders
                .Include(x => x.AppUser)
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(order => order.OrderId==orderId);
        }

    }
}