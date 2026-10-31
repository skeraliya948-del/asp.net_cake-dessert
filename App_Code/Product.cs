using System;

namespace SweetDelights
{
    public class Product
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public bool IsEggless { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public string Badge { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string FullDetails { get; set; }
        public string Ingredients { get; set; }
    }
}
