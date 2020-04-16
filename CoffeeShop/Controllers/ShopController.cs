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
using CoffeeShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CoffeeShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDBContext _context;
        private readonly CoffeeShopIdentityContext _identityContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ShopController(ShopDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]
        // GET: Shop
        public async Task<IActionResult> Index()
        {
            return View(await _context.Items.ToListAsync());
        }
        public async Task<IActionResult> Buy(int id)
        {
            
            IdentityUser iUser = await _userManager.GetUserAsync(HttpContext.User);
            Users user = _context.Users.FirstOrDefault(e => e.Email == iUser.Email);
            UserItems thisPurchase = new UserItems();
            thisPurchase.UserId = user.Id;
            thisPurchase.ItemId = id;
            TempData["itemId"] = id;
            TempData["userId"] = user.Id;
            
            BuyViewModel buyViewModel = new BuyViewModel() { Item = _context.Items.FirstOrDefault(e => e.Id == id), Balance = user.Balance, userItem = thisPurchase };
            return View(buyViewModel);
        }

        // POST: Shop/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy()
        {
            Users user = _context.Users.FirstOrDefault(e => e.Id == (int)TempData["userId"]);
            Items item = _context.Items.FirstOrDefault(e => e.Id == (int)TempData["itemId"]);
            user.Balance -= item.Price;
            user.Points++;

            _context.Update(user);
            _context.UserItems.Add(new UserItems() { ItemId = item.Id, UserId = user.Id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Shop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Shop/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemName,ItemDescription,Quantity,Price")] Items items)
        {
            if (ModelState.IsValid)
            {
                _context.Add(items);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(items);
        }

        // GET: Shop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        // POST: Shop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,ItemDescription,Quantity,Price")] Items items)
        {
            if (id != items.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(items);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsExists(items.Id))
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
            return View(items);
        }

        // GET: Shop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // POST: Shop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var items = await _context.Items.FindAsync(id);
            _context.Items.Remove(items);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
