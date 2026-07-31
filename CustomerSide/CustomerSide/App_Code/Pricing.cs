using System;
using System.Collections.Generic;

namespace Azure
{
    public static class Pricing
    {
        public const decimal IcedUpcharge = 10.00m;
        public const decimal LargeUpcharge = 15.00m;
        public const decimal DiscountRate = 0.20m;

        public static decimal UnitPrice(decimal basePrice, string temperature, string size)
        {
            decimal price = basePrice;

            if (string.Equals(temperature, "Iced", StringComparison.OrdinalIgnoreCase))
                price += IcedUpcharge;

            if (string.Equals(size, "L", StringComparison.OrdinalIgnoreCase))
                price += LargeUpcharge;

            return price;
        }

        public static List<decimal> DrinkPrices(List<CartItem> items)
        {
            List<decimal> prices = new List<decimal>();
            if (items == null) return prices;

            foreach (CartItem item in items)
            {
                for (int i = 0; i < item.Quantity; i++)
                    prices.Add(item.Price);
            }

            prices.Sort();
            prices.Reverse();
            return prices;
        }

        public static decimal DiscountFor(List<CartItem> items, int eligibleCount)
        {
            if (eligibleCount <= 0) return 0m;

            List<decimal> prices = DrinkPrices(items);
            int take = Math.Min(eligibleCount, prices.Count);

            decimal discount = 0m;
            for (int i = 0; i < take; i++)
                discount += decimal.Round(prices[i] * DiscountRate, 2);

            return discount;
        }

        public static List<decimal> DiscountSteps(List<CartItem> items)
        {
            List<decimal> steps = new List<decimal>();
            List<decimal> prices = DrinkPrices(items);

            decimal running = 0m;
            for (int i = 0; i < prices.Count; i++)
            {
                running += decimal.Round(prices[i] * DiscountRate, 2);
                steps.Add(running);
            }

            return steps;
        }

        public static string Peso(decimal amount)
        {
            return "&#8369;" + amount.ToString("0.00");
        }
    }
}
