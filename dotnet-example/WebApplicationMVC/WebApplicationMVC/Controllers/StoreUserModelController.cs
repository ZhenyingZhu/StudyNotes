using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class StoreUserModelController : Controller
    {
        private readonly SignInManager<StoreUserModel> signInManager;
        private readonly UserManager<StoreUserModel> userManager;
        private readonly IConfiguration config;

        public StoreUserModelController(
            SignInManager<StoreUserModel> signInManager,
            UserManager<StoreUserModel> userManager,
            IConfiguration config)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
        }

        public IActionResult Login()
        {
            // zhenying: when could this be called?
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(StoreUserModel model)
        {
            if (ModelState.IsValid)
            {
                // zhenying: In DutchTreat a new layer is created calls ViewModel. It has properties that are not store
                // in DB.
                var result = await this.signInManager.PasswordSignInAsync(
                    model.UserName, model.PasswordHash, true, false);

                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        //return RedirectToAction("Shop", "App");
                        return RedirectToAction("Index", "home");
                    }
                }
            }

            ModelState.AddModelError("", "Failed to login");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            //return RedirectToAction("Index", "app");
            return RedirectToAction("Index", "home");
        }

        // zhenying: the Route here with app at the begining is to make angular to work.
        [HttpPost("/[Controller]/CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] StoreUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var result = await this.signInManager.CheckPasswordSignInAsync(user, model.PasswordHash, false);
                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            this.config["Tokens:Issuer"],
                            this.config["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expriation = token.ValidTo
                        };
                        return Created("", results);
                    }
                }
            }

            return BadRequest();
        }
    }
}