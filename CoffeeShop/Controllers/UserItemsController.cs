using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CoffeeShop.Models.ViewModels;

namespace CoffeeShop.Controllers
{
    [Authorize(Roles ="Administrator, Manager, User")]
    public class UserItemsController : Controller
    {
        private readonly ShopDBContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserItemsController(ShopDBContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: UserItems
        public async Task<IActionResult> Index()
        {
            Users user = new ShopDBContext().Users.FirstOrDefault(e => e.Email == _userManager.GetUserName(User));
            if (_signInManager.IsSignedIn(User))
            {
                List<Items> dbItems = _context.Items.ToList();
                var userItems = _context.UserItems.Where(e => e.UserId == user.Id);

                List<Items> items = new List<Items>();
                foreach (var userItem in userItems)
                {
                    items.Add(dbItems.FirstOrDefault(e => e.Id == userItem.ItemId));
                }

                PurchaseHistoryViewModel purchases = new PurchaseHistoryViewModel();
                purchases.user = user;
                purchases.items = items;
                purchases.userItems = userItems.OrderByDescending(e => e.PurchaseDate).ToList();
                return View(purchases);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: UserItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userItems = await _context.UserItems
                .FirstOrDefaultAsync(m => m.UserItemId == id);
            if (userItems == null)
            {
                return NotFound();
            }

            return View(userItems);
        }

        // GET: UserItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserItemId,UserId,ItemId")] UserItems userItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userItems);
        }

        // GET: UserItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userItems = await _context.UserItems.FindAsync(id);
            if (userItems == null)
            {
                return NotFound();
            }
            return View(userItems);
        }

        // POST: UserItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserItemId,UserId,ItemId")] UserItems userItems)
        {
            if (id != userItems.UserItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserItemsExists(userItems.UserItemId))
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
            return View(userItems);
        }

        // GET: UserItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userItems = await _context.UserItems
                .FirstOrDefaultAsync(m => m.UserItemId == id);
            if (userItems == null)
            {
                return NotFound();
            }

            return View(userItems);
        }

        // POST: UserItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userItems = await _context.UserItems.FindAsync(id);
            _context.UserItems.Remove(userItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserItemsExists(int id)
        {
            return _context.UserItems.Any(e => e.UserItemId == id);
        }
    }
}
