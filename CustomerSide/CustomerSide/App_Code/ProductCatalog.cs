using System.Collections.Generic;
using System.Linq;

namespace Azure
{

    public static class ProductCatalog
    {
        public static List<Product> GetAll()
        {
            return new List<Product>
            {
                
                new Product
                {
                    Id = 1,
                    Name = "Cinnamon Cream Latte",
                    Category = "Coffee",
                    Tag = "Espresso",
                    Price = 150.00m,
                    Description = "Espresso, steamed milk, and a warm dusting of cinnamon.",
                    ImageUrl = "Images/CinnamonLatte.png",
                    Badge = "Best Seller"
                },
                new Product
                {
                    Id = 2,
                    Name = "Cappuccino",
                    Category = "Coffee",
                    Tag = "Espresso",
                    Price = 140.00m,
                    Description = "Bold espresso topped with thick, velvety steamed foam.",
                    ImageUrl = "Images/Cappuccino.png",
                    Badge = "Classic"
                },
                new Product
                {
                    Id = 3,
                    Name = "Salted Caramel Frappe",
                    Category = "Coffee",
                    Tag = "Cold Brew",
                    Price = 180.00m,
                    Description = "Blended cold brew, caramel, and a touch of sea salt cream.",
                    ImageUrl = "Images/SaltedCaramelFrappe.png",
                    Badge = "Seasonal"
                },
                new Product
                {
                    Id = 4,
                    Name = "Mocha Cream Latte",
                    Category = "Coffee",
                    Tag = "Tea & Cream",
                    Price = 165.00m,
                    Description = "Rich espresso and chocolate, finished with a cloud of cream.",
                    ImageUrl = "Images/MochaCreamLatte.png",
                    Badge = "Popular"
                },

                new Product
                {
                    Id = 5,
                    Name = "Matcha Cortado",
                    Category = "Tea",
                    Tag = "Tea & Cream",
                    Price = 160.00m,
                    Description = "Ceremonial-grade matcha layered with a touch of milk.",
                    ImageUrl = "Images/MatchaCortado.png",
                    Badge = "New"
                },
                new Product
                {
                    Id = 6,
                    Name = "Peach Iced Tea",
                    Category = "Tea",
                    Tag = "Seasonal",
                    Price = 140.00m,
                    Description = "Black tea steeped with real peach, served over ice.",
                    ImageUrl = "Images/PeachIcedTea.png",
                    Badge = "Seasonal"
                }
            };
        }

        public static Product GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public static List<Product> GetByCategory(string category)
        {
            return GetAll().Where(p => p.Category == category).ToList();
        }
    }
}