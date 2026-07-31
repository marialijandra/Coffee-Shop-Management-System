using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Azure
{

    public static class ProductCatalog
    {
        public static List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            Database db = new Database();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string sql = @"
SELECT DrinkID, DrinkName, Category, Tag, BasePrice, Description, ImageUrl, Badge, IsSoldOut
FROM Drinks
ORDER BY DisplayOrder, DrinkID";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Product p = new Product();

                        p.Id = r.GetInt32("DrinkID");
                        p.Name = r.GetString("DrinkName");
                        p.Category = r.GetString("Category");
                        p.Tag = r.IsDBNull(r.GetOrdinal("Tag")) ? "" : r.GetString("Tag");
                        p.Price = r.GetDecimal("BasePrice");
                        p.Description = r.IsDBNull(r.GetOrdinal("Description")) ? "" : r.GetString("Description");
                        p.ImageUrl = r.IsDBNull(r.GetOrdinal("ImageUrl")) ? "" : r.GetString("ImageUrl");
                        p.Badge = r.IsDBNull(r.GetOrdinal("Badge")) ? "" : r.GetString("Badge");
                        p.IsSoldOut = r.GetBoolean("IsSoldOut");

                        products.Add(p);
                    }
                }
            }

            return products;
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
