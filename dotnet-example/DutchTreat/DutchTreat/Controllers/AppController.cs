using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }
            else
            {

            }

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }

        public IActionResult Error()
        {
            throw new InvalidOperationException("Something wrong.");
        }
    }
}
