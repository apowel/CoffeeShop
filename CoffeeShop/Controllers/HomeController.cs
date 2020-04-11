using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDBContext _context = new ShopDBContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        /*public IActionResult Register()
        {
            return View();
        }*//*
        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                var model = _context.Users.FirstOrDefault(e => e.Id == user.Id);
                return RedirectToAction("details", model);
            }
            return View();
        }*/
        public ViewResult Welcome(int? id)
        {
            return View(_context.Users.FirstOrDefault(e => e.Id == id));
        }
        public ViewResult Details(int? id)
        {
            ViewBag.PageTitle = "User Details";
            return View(_context.Users.FirstOrDefault(e => e.Id == id));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
