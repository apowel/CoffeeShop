using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models.ViewModels
{
    public class BuyViewModel
    {
        public Items Item { get; set; }
        public int UserId { get; set; }
        public decimal? Balance { get; set; }
        public UserItems userItem { get; set; }
    }
}
