using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure
{

    public static class OrderStore
    {
        private static readonly object _lock = new object();
        private static List<Order> _orders;
        private static int _nextId = 926;
        private static readonly string[] _tableLetters = { "A", "B", "C" };
        private static int _tableCounter = 3;

        private static List<Order> Orders
        {
            get
            {
                if (_orders == null) _orders = SeedOrders();
                return _orders;
            }
        }

        public static List<Order> GetAll()
        {
            lock (_lock)
            {
                return Orders.OrderByDescending(o => o.CreatedAt).ThenByDescending(o => o.Id).ToList();
            }
        }

        public static Order GetById(int id)
        {
            lock (_lock)
            {
                return Orders.FirstOrDefault(o => o.Id == id);
            }
        }

        public static Order Add(string customerName, string orderType, List<CartItem> items,
            decimal subtotal, decimal discount, decimal total)
        {
            lock (_lock)
            {
                var order = new Order
                {
                    Id = _nextId++,
                    RefCode = NextRefCode(orderType),
                    CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Guest" : customerName,
                    OrderType = orderType,
                    Items = items,
                    Subtotal = subtotal,
                    Discount = discount,
                    Total = total,
                    Status = Order.StatusNew,
                    IsPaid = false,
                    CreatedAt = DateTime.Now
                };
                Orders.Add(order);
                return order;
            }
        }

        private static string NextRefCode(string orderType)
        {
            if (orderType == "Takeout") return "TA";
            _tableCounter++;
            string letter = _tableLetters[_tableCounter % _tableLetters.Length];
            int num = 1 + (_tableCounter % 9);
            return letter + num;
        }

        public static void SetStatus(int id, string status)
        {
            lock (_lock)
            {
                var order = Orders.FirstOrDefault(o => o.Id == id);
                if (order != null) order.Status = status;
            }
        }

        public static void MarkPaid(int id, string method, bool discountValidated)
        {
            lock (_lock)
            {
                var order = Orders.FirstOrDefault(o => o.Id == id);
                if (order == null) return;

                order.IsPaid = true;
                order.PaymentMethod = method;
                order.DiscountValidated = discountValidated;

                if (order.DiscountRequested && !discountValidated)
                {
                    order.Discount = 0m;
                    order.Total = order.Subtotal;
                }

                if (order.Status == Order.StatusNew)
                {
                    order.Status = Order.StatusInProgress;
                }
            }
        }

        private static List<Order> SeedOrders()
        {
            var now = DateTime.Now;

            var list = new List<Order>
            {
                new Order
                {
                    Id = 920,
                    RefCode = "A4",
                    CustomerName = "Ariel Hikmat",
                    OrderType = "Dine In",
                    Items = new List<CartItem>
                    {
                        Item(1, "Cinnamon Latte", 150m, "Images/CinnamonLatte.png", "Hot", "L", 2),
                        Item(5, "Matcha Cortado", 160m, "Images/MatchaCortado.png", "Iced", "S", 1)
                    },
                    Subtotal = 500m, Discount = 0m, Total = 500m,
                    Status = Order.StatusServed, IsPaid = true, PaymentMethod = "Cash",
                    CreatedAt = now.AddMinutes(-48)
                },
                new Order
                {
                    Id = 921,
                    RefCode = "B2",
                    CustomerName = "Denis Freeman",
                    OrderType = "Dine In",
                    Items = new List<CartItem>
                    {
                        Item(2, "Cappuccino", 140m, "Images/Cappuccino.png", "Hot", "S", 2),
                        Item(6, "Peach Iced Tea", 140m, "Images/PeachIcedTea.png", "Iced", "S", 1)
                    },
                    Subtotal = 420m, Discount = 0m, Total = 420m,
                    Status = Order.StatusInProgress, IsPaid = true, PaymentMethod = "Card",
                    CreatedAt = now.AddMinutes(-40)
                },
                new Order
                {
                    Id = 916,
                    RefCode = "TA",
                    CustomerName = "Morgan Cox",
                    OrderType = "Takeout",
                    Items = new List<CartItem>
                    {
                        Item(4, "Mocha Cream Latte", 165m, "Images/MochaCreamLatte.png", "Iced", "S", 1),
                        Item(3, "Salted Caramel Frappe", 180m, "Images/SaltedCaramelFrappe.png", "Iced", "S", 2)
                    },
                    Subtotal = 525m, Discount = 105m, Total = 420m,
                    Status = Order.StatusNew, IsPaid = false,
                    CreatedAt = now.AddMinutes(-33)
                },
                new Order
                {
                    Id = 919,
                    RefCode = "TA",
                    CustomerName = "Paul Rey",
                    OrderType = "Takeout",
                    Items = new List<CartItem>
                    {
                        Item(6, "Peach Iced Tea", 140m, "Images/PeachIcedTea.png", "Iced", "S", 2),
                        Item(1, "Cinnamon Latte", 150m, "Images/CinnamonLatte.png", "Hot", "S", 1)
                    },
                    Subtotal = 430m, Discount = 0m, Total = 430m,
                    Status = Order.StatusInProgress, IsPaid = true, PaymentMethod = "Cash",
                    CreatedAt = now.AddMinutes(-25)
                },
                new Order
                {
                    Id = 912,
                    RefCode = "C2",
                    CustomerName = "Maja Becker",
                    OrderType = "Dine In",
                    Items = new List<CartItem>
                    {
                        Item(5, "Matcha Cortado", 160m, "Images/MatchaCortado.png", "Hot", "S", 1),
                        Item(2, "Cappuccino", 140m, "Images/Cappuccino.png", "Hot", "S", 1),
                        Item(4, "Mocha Cream Latte", 165m, "Images/MochaCreamLatte.png", "Iced", "S", 2)
                    },
                    Subtotal = 630m, Discount = 0m, Total = 630m,
                    Status = Order.StatusServed, IsPaid = true, PaymentMethod = "Card",
                    CreatedAt = now.AddMinutes(-70)
                },
                new Order
                {
                    Id = 908,
                    RefCode = "A9",
                    CustomerName = "Erwan Richard",
                    OrderType = "Dine In",
                    Items = new List<CartItem>
                    {
                        Item(3, "Salted Caramel Frappe", 180m, "Images/SaltedCaramelFrappe.png", "Iced", "S", 1),
                        Item(6, "Peach Iced Tea", 140m, "Images/PeachIcedTea.png", "Iced", "S", 1)
                    },
                    Subtotal = 320m, Discount = 0m, Total = 320m,
                    Status = Order.StatusServed, IsPaid = true, PaymentMethod = "Cash",
                    CreatedAt = now.AddMinutes(-95)
                },
                new Order
                {
                    Id = 917,
                    RefCode = "TA",
                    CustomerName = "Sophie Tan",
                    OrderType = "Takeout",
                    Items = new List<CartItem>
                    {
                        Item(4, "Mocha Cream Latte", 165m, "Images/MochaCreamLatte.png", "Hot", "S", 1)
                    },
                    Subtotal = 165m, Discount = 0m, Total = 165m,
                    Status = Order.StatusCancelled, IsPaid = false,
                    CreatedAt = now.AddMinutes(-30)
                }
            };

            return list;
        }

        private static CartItem Item(int productId, string name, decimal basePrice, string image,
            string temp, string size, int qty)
        {
            decimal unit = basePrice + (size == "L" ? 20.00m : 0m);
            return new CartItem
            {
                ProductId = productId,
                Name = name,
                Price = unit,
                ImageUrl = image,
                Temperature = temp,
                Size = size,
                Quantity = qty
            };
        }
    }
}
