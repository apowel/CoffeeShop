using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    public class UserItemsController : Controller
    {
        private readonly ShopDBContext _context;

        public UserItemsController(ShopDBContext context)
        {
            _context = context;
        }

        // GET: UserItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserItems.ToListAsync());
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
