using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySqlConnector;

namespace Azure
{
    public static class OrderStore
    {
        private const string SelectOrder =
            "SELECT OrderID, OrderNumber, RefCode, CustomerName, OrderType, PaymentMethod, Subtotal, " +
            "       DiscountApplied, DiscountQuantity, TotalPrice, AmountPaid, OrderStatus, IsPaid, OrderDateTime " +
            "FROM Orders ";

        public static List<Order> GetAll()
        {
            var orders = new List<Order>();

            using (var connection = Db.Open())
            {
                using (var command = Db.Command(connection,
                    SelectOrder + "ORDER BY OrderDateTime DESC, OrderID DESC"))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) orders.Add(MapOrder(reader));
                }

                LoadItems(connection, orders);
            }

            return orders;
        }

        public static List<Order> GetForDate(DateTime day)
        {
            var orders = new List<Order>();

            using (var connection = Db.Open())
            {
                using (var command = Db.Command(connection,
                    SelectOrder + "WHERE DATE(OrderDateTime) = @day ORDER BY OrderDateTime DESC, OrderID DESC"))
                {
                    command.Parameters.AddWithValue("@day", day.Date);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) orders.Add(MapOrder(reader));
                    }
                }

                LoadItems(connection, orders);
            }

            return orders;
        }

        public static Order GetById(int id)
        {
            using (var connection = Db.Open())
            {
                Order order = null;

                using (var command = Db.Command(connection, SelectOrder + "WHERE OrderID = @id"))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) order = MapOrder(reader);
                    }
                }

                if (order != null) LoadItems(connection, new List<Order> { order });
                return order;
            }
        }

        public static void SetStatus(int id, string status)
        {
            using (var connection = Db.Open())
            using (var command = Db.Command(connection,
                "UPDATE Orders SET OrderStatus = @status WHERE OrderID = @id"))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public static bool CanTransition(Order order, string target)
        {
            if (order == null || string.IsNullOrEmpty(target)) return false;
            if (order.Status == target) return false;
            if (order.StatusLocked) return false;
            if (target == Order.StatusNew) return false;

            if (order.Status == Order.StatusNew)
            {
                if (target == Order.StatusCancelled) return true;
                return order.IsPaid && target == Order.StatusInProgress;
            }

            return target == Order.StatusServed;
        }

        public static bool TrySetStatus(int id, string target)
        {
            var order = GetById(id);
            if (!CanTransition(order, target)) return false;

            SetStatus(id, target);
            return true;
        }

        public static void MarkPaid(int id, string method, int discountQuantity)
        {
            var order = GetById(id);
            if (order == null || order.IsPaid) return;
            if (order.Status == Order.StatusCancelled) return;

            int eligible = Math.Max(0, discountQuantity);
            decimal discount = Pricing.DiscountFor(order.Subtotal, order.ItemCount, eligible);
            if (eligible > order.ItemCount) eligible = order.ItemCount;

            decimal total = order.Subtotal - discount;
            decimal amountPaid = total;

            string nextStatus = order.Status == Order.StatusNew ? Order.StatusInProgress : order.Status;

            using (var connection = Db.Open())
            using (var command = Db.Command(connection,
                "UPDATE Orders SET " +
                "  IsPaid = 1, PaymentMethod = @method, DiscountApplied = @discount, " +
                "  DiscountQuantity = @discountQty, TotalPrice = @total, AmountPaid = @amountPaid, " +
                "  OrderStatus = @status " +
                "WHERE OrderID = @id"))
            {
                command.Parameters.AddWithValue("@method", method);
                command.Parameters.AddWithValue("@discount", discount);
                command.Parameters.AddWithValue("@discountQty", eligible);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@amountPaid", amountPaid);
                command.Parameters.AddWithValue("@status", nextStatus);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        private static void LoadItems(MySqlConnection connection, List<Order> orders)
        {
            if (orders.Count == 0) return;

            var byId = orders.ToDictionary(o => o.Id);
            string idList = string.Join(",", byId.Keys);

            using (var command = Db.Command(connection,
                "SELECT OrderID, DrinkID, DrinkName, Temperature, Size, Quantity, PricePerDrink, ImageUrl " +
                "FROM OrderDetails WHERE OrderID IN (" + idList + ") ORDER BY OrderDetailID"))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int orderId = Db.Int(reader, "OrderID");
                    Order order;
                    if (!byId.TryGetValue(orderId, out order)) continue;

                    order.Items.Add(new CartItem
                    {
                        ProductId = Db.Int(reader, "DrinkID"),
                        Name = Db.Str(reader, "DrinkName"),
                        Price = Db.Dec(reader, "PricePerDrink"),
                        ImageUrl = Db.Str(reader, "ImageUrl"),
                        Temperature = Db.Str(reader, "Temperature"),
                        Size = Db.Str(reader, "Size"),
                        Quantity = Db.Int(reader, "Quantity")
                    });
                }
            }
        }

        private static Order MapOrder(IDataRecord row)
        {
            return new Order
            {
                Id = Db.Int(row, "OrderID"),
                OrderNumber = Db.Str(row, "OrderNumber"),
                RefCode = Db.Str(row, "RefCode"),
                CustomerName = Db.Str(row, "CustomerName"),
                OrderType = Db.Str(row, "OrderType"),
                PaymentMethod = Db.Str(row, "PaymentMethod"),
                Subtotal = Db.Dec(row, "Subtotal"),
                DiscountApplied = Db.Dec(row, "DiscountApplied"),
                DiscountQuantity = Db.Int(row, "DiscountQuantity"),
                StoredTotal = Db.Dec(row, "TotalPrice"),
                AmountPaid = Db.DecOrNull(row, "AmountPaid"),
                Status = Db.Str(row, "OrderStatus"),
                IsPaid = Db.Bool(row, "IsPaid"),
                CreatedAt = Db.Date(row, "OrderDateTime"),
                Items = new List<CartItem>()
            };
        }
    }
}
