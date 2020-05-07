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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CoffeeShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDBContext _context;
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

            Users user = new ShopDBContext().Users.FirstOrDefault(e => e.Email == _userManager.GetUserName(User));
            UserItems thisPurchase = new UserItems();
            
            thisPurchase.UserId = user.Id;
            thisPurchase.ItemId = id;

            HttpContext.Session.Set("thisPurchase", ObjectToByteArray(thisPurchase));
            /*In case I decide to go back to using TempData instead of serializing the UserItems object.
             * 
             * TempData["itemId"] = id;
            TempData["userId"] = user.Id;*/

            BuyViewModel buyViewModel = new BuyViewModel() { Item = _context.Items.FirstOrDefault(e => e.Id == id), Balance = user.Balance, userItem = thisPurchase };
            return View(buyViewModel);
        }

        // POST: Shop/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy()
        {
            /*In case I decide to go back to using TempData.
             * 
             * Users user = _context.Users.FirstOrDefault(e => e.Id == (int)TempData["userId"]);
            Items item = _context.Items.FirstOrDefault(e => e.Id == (int)TempData["itemId"]);*/
            
            UserItems thisPurchase = (UserItems)ByteArrayToObject(HttpContext.Session.Get("thisPurchase"));
            thisPurchase.PurchaseDate = DateTime.Now;
            Users user = _context.Users.FirstOrDefault(e => e.Id == thisPurchase.UserId);
            Items item = _context.Items.FirstOrDefault(e => e.Id == thisPurchase.ItemId);
            user.Balance -= item.Price;
            user.Points++;
            _context.Update(user);
            _context.UserItems.Add(thisPurchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
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
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
