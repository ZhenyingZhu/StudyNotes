using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _context;

        public AppController(IMailService mailService, DutchContext context)
        {
            this._mailService = mailService;
            this._context = context;
        }

        public IActionResult Index()
        {
            var results = _context.Products.ToList();

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
                _mailService.SendMessage("zz2283@columbia.edu", model.Subject, $"From: {model.Username} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            else
            {
                // Should show error here. But let the View do the job.
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

        public IActionResult Shop()
        {
            List<Data.Entities.Product> results = _context.Products
                .OrderBy(p => p.Category)
                .ToList();

            //var results = from p in _context.Products orderby p.Category select p;

            return View(results);
        }
    }
}
