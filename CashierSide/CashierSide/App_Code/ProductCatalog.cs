using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace Azure
{
    public static class ProductCatalog
    {
        private const string SelectColumns =
            "SELECT DrinkID, DrinkName, Category, Tag, BasePrice, Description, ImageUrl, Badge, IsSoldOut " +
            "FROM Drinks ";

        public static List<Product> GetAll()
        {
            var products = new List<Product>();

            using (var connection = Db.Open())
            using (var command = Db.Command(connection, SelectColumns + "ORDER BY DisplayOrder, DrinkID"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read()) products.Add(Map(reader));
            }

            return products;
        }

        public static Product GetById(int id)
        {
            using (var connection = Db.Open())
            using (var command = Db.Command(connection, SelectColumns + "WHERE DrinkID = @id"))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? Map(reader) : null;
                }
            }
        }

        public static List<Product> GetByCategory(string category)
        {
            var products = new List<Product>();

            using (var connection = Db.Open())
            using (var command = Db.Command(connection,
                SelectColumns + "WHERE Category = @category ORDER BY DisplayOrder, DrinkID"))
            {
                command.Parameters.AddWithValue("@category", category);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) products.Add(Map(reader));
                }
            }

            return products;
        }

        public static bool IsSoldOut(int id)
        {
            using (var connection = Db.Open())
            using (var command = Db.Command(connection,
                "SELECT IsSoldOut FROM Drinks WHERE DrinkID = @id"))
            {
                command.Parameters.AddWithValue("@id", id);
                object result = command.ExecuteScalar();
                return result != null && result != System.DBNull.Value && System.Convert.ToBoolean(result);
            }
        }

        public static void SetSoldOut(int id, bool soldOut)
        {
            using (var connection = Db.Open())
            using (var command = Db.Command(connection,
                "UPDATE Drinks SET IsSoldOut = @soldOut WHERE DrinkID = @id"))
            {
                command.Parameters.AddWithValue("@soldOut", soldOut ? 1 : 0);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private static Product Map(IDataRecord row)
        {
            return new Product
            {
                Id = Db.Int(row, "DrinkID"),
                Name = Db.Str(row, "DrinkName"),
                Category = Db.Str(row, "Category"),
                Tag = Db.Str(row, "Tag"),
                Price = Db.Dec(row, "BasePrice"),
                Description = Db.Str(row, "Description"),
                ImageUrl = Db.Str(row, "ImageUrl"),
                Badge = Db.Str(row, "Badge"),
                IsSoldOut = Db.Bool(row, "IsSoldOut")
            };
        }
    }
}
