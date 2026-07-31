using System;

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

        public static decimal DiscountPerDrink(decimal subtotal, int drinkCount)
        {
            if (drinkCount <= 0 || subtotal <= 0m) return 0m;
            return decimal.Round(DiscountRate * subtotal / drinkCount, 2);
        }

        public static decimal DiscountFor(decimal subtotal, int drinkCount, int eligibleCount)
        {
            if (eligibleCount <= 0) return 0m;
            if (eligibleCount > drinkCount) eligibleCount = drinkCount;

            decimal discount = DiscountPerDrink(subtotal, drinkCount) * eligibleCount;
            return discount > subtotal ? subtotal : discount;
        }

        public static string Peso(decimal amount)
        {
            return "&#8369;" + amount.ToString("0.00");
        }
    }
}
