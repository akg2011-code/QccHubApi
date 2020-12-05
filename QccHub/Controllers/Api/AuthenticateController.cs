using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QccHub.Data;
using QccHub.DTOS;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext Context;
        UserManager<User> UserManager;
        RoleManager<IdentityRole> RoleManager;
        SignInManager<User> SignInManager;
        public AuthenticateController(ApplicationDbContext _Context
            , UserManager<User> _userManager
            , RoleManager<IdentityRole> _roleManager,
            SignInManager<User> _SignInManager
            )
        {
            Context = _Context;
            UserManager = _userManager;
            RoleManager = _roleManager;
            SignInManager = _SignInManager;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisteration user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.FindByNameAsync(user.UserName);
                if (result != null)
                    return BadRequest("there is a user with this information");
                else
                {
                    User newUser = new User
                    {
                        UserName = user.UserName,
                        NormalizedUserName = user.UserName.ToUpper(),
                        Email = user.Email,
                        NormalizedEmail = user.Email.ToUpper(),
                        EmailConfirmed = true,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = true,
                        LockoutEnabled = false,
                        AccessFailedCount = 1,
                    };

                    var result2 = await UserManager.CreateAsync(newUser, user.Password);
                    if (result2.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(newUser, "User");
                        return Ok("user registered successfully");
                    }
                    else
                    {
                        return BadRequest(result2.Errors.ToList()[0].Description);
                    }
                }
            }
            return BadRequest("Invalid Form");

        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany([FromBody]UserRegisteration user)
        {
            if (ModelState.IsValid)
            {
                var result = await Context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber || u.Email == user.Email);
                if (result != null)
                    return BadRequest("there is a user with this information");
                else
                {
                    User newUser = new User
                    {
                        UserName = user.UserName,
                        NormalizedUserName = user.UserName.ToUpper(),
                        Email = user.Email,
                        NormalizedEmail = user.Email.ToUpper(),
                        EmailConfirmed = true,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = true,
                        LockoutEnabled = false,
                        AccessFailedCount = 1,
                        IsTrusted = false
                    };

                    var result2 = await UserManager.CreateAsync(newUser, user.Password);
                    if (result2.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(newUser, "Company");
                        return Ok("user registered successfully");
                    }
                    else
                    {
                        return BadRequest(result2.Errors.ToList()[0].Description);
                    }
                }
            }
            return BadRequest("Invalid Form");

        }

    }
}