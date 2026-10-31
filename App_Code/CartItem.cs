using System;

namespace SweetDelights
{
    public class CartItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string CustomDetails { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Price * Quantity;
            }
        }
    }

    public class User
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
