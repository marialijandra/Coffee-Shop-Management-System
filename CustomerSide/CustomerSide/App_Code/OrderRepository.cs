using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Azure
{
    public class OrderRepository
    {
        public void SaveOrder(
            string orderNumber,
            string customerName,
            string orderType,
            decimal subtotal,
            decimal discount,
            int discountQty,
            decimal total,
            List<CartItem> cart)
        {
            Database db = new Database();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Generate a simple 8-character reference code
                    string refCode = Guid.NewGuid()
                        .ToString("N")
                        .Substring(0, 8)
                        .ToUpper();

                    string orderSql = @"
INSERT INTO Orders
(
    OrderNumber,
    RefCode,
    CustomerName,
    PaymentMethod,
    OrderType,
    Subtotal,
    DiscountApplied,
    DiscountQuantity,
    TotalPrice,
    AmountPaid,
    IsPaid,
    OrderStatus
)
VALUES
(
    @OrderNumber,
    @RefCode,
    @CustomerName,
    @PaymentMethod,
    @OrderType,
    @Subtotal,
    @Discount,
    @DiscountQty,
    @Total,
    @AmountPaid,
    @IsPaid,
    'New Order'
);";

                    MySqlCommand cmd = new MySqlCommand(orderSql, conn, trans);

                    cmd.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    cmd.Parameters.AddWithValue("@RefCode", refCode);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);

                    // Customer side only places orders.
                    cmd.Parameters.AddWithValue("@PaymentMethod", DBNull.Value);

                    cmd.Parameters.AddWithValue("@OrderType",
                        orderType == "Dine In" ? "Dine-in" : "Take-out");

                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@Discount", discount);
                    cmd.Parameters.AddWithValue("@DiscountQty", discountQty);
                    cmd.Parameters.AddWithValue("@Total", total);

                    // Customer hasn't paid yet
                    cmd.Parameters.AddWithValue("@AmountPaid", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPaid", false);

                    cmd.ExecuteNonQuery();

                    int orderId = Convert.ToInt32(cmd.LastInsertedId);

                    foreach (CartItem item in cart)
                    {
                        string detailSql = @"
INSERT INTO OrderDetails
(
    OrderID,
    DrinkID,
    DrinkName,
    Description,
    Temperature,
    Size,
    Quantity,
    PricePerDrink,
    ImageUrl
)
VALUES
(
    @OrderID,
    @DrinkID,
    @DrinkName,
    @Description,
    @Temperature,
    @Size,
    @Quantity,
    @PricePerDrink,
    @ImageUrl
);";

                        MySqlCommand detail = new MySqlCommand(detailSql, conn, trans);

                        detail.Parameters.AddWithValue("@OrderID", orderId);
                        detail.Parameters.AddWithValue("@DrinkID", item.ProductId);
                        detail.Parameters.AddWithValue("@DrinkName", item.Name);

                        // CartItem doesn't contain a description,
                        // so leave it empty.
                        detail.Parameters.AddWithValue("@Description", "");

                        detail.Parameters.AddWithValue("@Temperature", item.Temperature);
                        detail.Parameters.AddWithValue("@Size", item.Size);
                        detail.Parameters.AddWithValue("@Quantity", item.Quantity);
                        detail.Parameters.AddWithValue("@PricePerDrink", item.Price);
                        detail.Parameters.AddWithValue("@ImageUrl", item.ImageUrl);

                        detail.ExecuteNonQuery();
                    }

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}