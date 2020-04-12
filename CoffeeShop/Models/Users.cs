using System;
using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal? Balance { get; set; }
        public int? Points { get; set; }
        public bool? Notifications { get; set; }
    }
}
