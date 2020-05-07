using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models.ViewModels
{
    public class PurchaseHistoryViewModel
    {
        public List<UserItems> userItems { get; set; }
        public List<Items> items { get; set; }
        public Users user { get; set; }
    }
}
