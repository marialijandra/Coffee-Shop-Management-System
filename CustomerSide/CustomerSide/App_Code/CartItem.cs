using System;

namespace Azure
{
    [Serializable]
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }         
        public string ImageUrl { get; set; }
        public string Temperature { get; set; }   
        public string Size { get; set; }            
        public int Quantity { get; set; }

        public decimal LineTotal
        {
            get { return Price * Quantity; }
        }

        public string FormattedPrice
        {
            get { return "&#8369;" + Price.ToString("0.00"); }
        }

        public string FormattedLineTotal
        {
            get { return "&#8369;" + LineTotal.ToString("0.00"); }
        }

        public string OptionsLabel
        {
            get { return Temperature + " &middot; " + Size; }
        }
    }
}
