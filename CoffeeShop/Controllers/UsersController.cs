using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using CoffeeShop.Data;
using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Controllers
{
    //[Authorize] makes authorization required for whole controller
    [Authorize(Roles = "Administrator, Manager")]
    public class UsersController : Controller
    {
        private readonly ShopDBContext _context;
        private readonly CoffeeShopIdentityContext _identityContext;

        public UsersController(ShopDBContext context)
        {
            _context = context;
        }
        
        // GET: Users
        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = _context.Users
                .FirstOrDefault(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,UserName,FirstName,LastName,Email,Phone,UserPassword,ConfirmPassword,Points,Notifications")] Users users)
        {
            if (ModelState.IsValid)
            {
                IdentityUser iUser = new IdentityUser() { UserName = users.Email, Email = users.Email, NormalizedEmail = users.Email.ToUpper(), PhoneNumber = users.Phone };
                _identityContext.Add(iUser);
                _identityContext.SaveChanges();
                _context.Add(users);
                _context.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }
        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = _context.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,UserName,FirstName,LastName,Email,Phone,UserPassword,ConfirmPassword,Points,Notifications")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = _context.Users
                .FirstOrDefault(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var users = _context.Users.Find(id);
            _context.Users.Remove(users);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
