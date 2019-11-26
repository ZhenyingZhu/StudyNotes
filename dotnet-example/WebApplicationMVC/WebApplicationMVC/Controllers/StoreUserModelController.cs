using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "home");
            }

            return View();
        }
    }
}