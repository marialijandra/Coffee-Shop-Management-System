using System;

namespace Azure
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Tag { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Badge { get; set; }
        public bool IsSoldOut { get; set; }

        public string FormattedPrice
        {
            get { return "&#8369;" + Price.ToString("0.00"); }
        }

        public bool IsAvailable
        {
            get { return !IsSoldOut; }
        }

        public string CardCssClass
        {
            get { return IsSoldOut ? "product-card sold-out" : "product-card"; }
        }
    }
}
