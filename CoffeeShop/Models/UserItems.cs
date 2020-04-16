using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class UserItems
    {
        [Key]
        public int UserItemId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
    }
}
