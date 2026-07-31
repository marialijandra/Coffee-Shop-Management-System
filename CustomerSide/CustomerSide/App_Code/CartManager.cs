using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure
{

    public static class CartManager
    {
        private const string SessionKey = "AzureCart";
        private const decimal LargeUpcharge = 15.00m;
        private const decimal IcedUpcharge = 10.00m;

        public static List<CartItem> GetCart()
        {
            var cart = HttpContext.Current.Session[SessionKey] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                HttpContext.Current.Session[SessionKey] = cart;
            }
            return cart;
        }

        public static void AddItem(Product product, int quantity, string temperature, string size)
        {
            var cart = GetCart();
            decimal unitPrice = product.Price + (size == "L" ? LargeUpcharge : 0m)
                                    + (temperature == "Iced" ? IcedUpcharge : 0m);

            var existing = cart.FirstOrDefault(c =>
                c.ProductId == product.Id &&
                c.Temperature == temperature &&
                c.Size == size);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = unitPrice,
                    ImageUrl = product.ImageUrl,
                    Temperature = temperature,
                    Size = size,
                    Quantity = quantity
                });
            }
        }

        public static void ChangeQuantity(int lineIndex, int delta)
        {
            var cart = GetCart();
            if (lineIndex < 0 || lineIndex >= cart.Count) return;

            cart[lineIndex].Quantity += delta;
            if (cart[lineIndex].Quantity <= 0)
                cart.RemoveAt(lineIndex);
        }

        public static void RemoveAt(int lineIndex)
        {
            var cart = GetCart();
            if (lineIndex < 0 || lineIndex >= cart.Count) return;
            cart.RemoveAt(lineIndex);
        }

        public static void Clear()
        {
            GetCart().Clear();
        }

        public static int ItemCount()
        {
            return GetCart().Sum(c => c.Quantity);
        }

        public static decimal Subtotal()
        {
            return GetCart().Sum(c => c.LineTotal);
        }
    }


    public static class OrderManager
    {
        public static int GetNextCustomerNumber()
        {
            Database db = new Database();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = "SELECT IFNULL(MAX(OrderID), 0) + 1 FROM Orders";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
