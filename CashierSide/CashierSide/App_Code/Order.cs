using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure
{
    [Serializable]
    public class Order
    {
        public const string StatusNew = OrderStatus.New;
        public const string StatusInProgress = OrderStatus.InProgress;
        public const string StatusServed = OrderStatus.Served;
        public const string StatusCancelled = OrderStatus.Cancelled;

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string RefCode { get; set; }
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
        public List<CartItem> Items { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountApplied { get; set; }
        public int DiscountQuantity { get; set; }
        public decimal StoredTotal { get; set; }
        public decimal? AmountPaid { get; set; }

        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

        public decimal Total
        {
            get { return IsPaid ? StoredTotal : Subtotal; }
        }

        public decimal Discount
        {
            get
            {
                if (Status == StatusNew) return Pricing.DiscountFor(Items, 1);
                return IsPaid ? DiscountApplied : 0m;
            }
        }

        public string DiscountStepsRaw
        {
            get
            {
                var parts = new List<string>();
                foreach (decimal step in Pricing.DiscountSteps(Items))
                    parts.Add(Raw(step));
                return string.Join(",", parts.ToArray());
            }
        }

        public decimal AppliedDiscount
        {
            get { return IsPaid ? DiscountApplied : 0m; }
        }

        public decimal PayableTotal { get { return Subtotal - AppliedDiscount; } }

        public int ItemCount
        {
            get { return Items == null ? 0 : Items.Sum(i => i.Quantity); }
        }

        private const int MaxDisplayLines = 2;

        public List<CartItem> DisplayItems
        {
            get { return Items == null ? new List<CartItem>() : Items.Take(MaxDisplayLines).ToList(); }
        }

        public int MoreItemsCount
        {
            get { return Items == null ? 0 : Math.Max(0, Items.Count - MaxDisplayLines); }
        }

        public bool HasMoreItems { get { return MoreItemsCount > 0; } }

        public string FormattedSubtotal { get { return Pricing.Peso(Subtotal); } }
        public string FormattedDiscount { get { return Discount > 0 ? "- " + Pricing.Peso(Discount) : Pricing.Peso(0m); } }
        public string FormattedTotal { get { return Pricing.Peso(Total); } }
        public string FormattedAmountPaid { get { return Pricing.Peso(AmountPaid ?? Total); } }
        public string FormattedPayableTotal { get { return Pricing.Peso(PayableTotal); } }
        public string FormattedAppliedDiscount { get { return AppliedDiscount > 0 ? "- " + Pricing.Peso(AppliedDiscount) : Pricing.Peso(0m); } }
        public string FormattedDate { get { return CreatedAt.ToString("MMM d, yyyy"); } }
        public string FormattedTime { get { return CreatedAt.ToString("hh:mm tt"); } }

        public bool DiscountRequested { get { return Discount > 0; } }

        public string SubtotalRaw { get { return Raw(Subtotal); } }
        public string TotalRaw { get { return Raw(Total); } }
        public string DiscountRaw { get { return Raw(Discount); } }

        private static string Raw(decimal value)
        {
            return value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        public string StatusCssClass
        {
            get
            {
                if (Status == StatusInProgress) return "status-progress";
                if (Status == StatusServed) return "status-served";
                if (Status == StatusCancelled) return "status-cancelled";
                return "status-new";
            }
        }

        public string StatusSubLabel
        {
            get
            {
                if (Status == StatusInProgress) return "Being prepared";
                if (Status == StatusServed) return "Order has been served";
                if (Status == StatusCancelled) return "Order cancelled";
                return "Waiting to start";
            }
        }

        public bool ShowPaymentButton { get { return Status == StatusNew; } }
        public bool ShowDetailsButton { get { return Status != StatusNew; } }

        public string ShortNumber { get { return (Id % 100).ToString(); } }

        public string RefCssClass
        {
            get
            {
                if (Status == StatusInProgress) return "order-ref ref-progress";
                if (Status == StatusServed) return "order-ref ref-served";
                if (Status == StatusCancelled) return "order-ref ref-cancelled";
                return "order-ref ref-new";
            }
        }

        public string CardCssClass
        {
            get
            {
                if (Status == StatusInProgress) return "order-card card-progress";
                if (Status == StatusServed) return "order-card card-served";
                if (Status == StatusCancelled) return "order-card card-cancelled";
                return "order-card card-new";
            }
        }

        public bool CanCancel { get { return Status == StatusNew || Status == StatusCancelled; } }

        public bool ShowNewOrderStep { get { return Status == StatusNew; } }

        public bool StatusLocked { get { return Status == StatusServed || Status == StatusCancelled; } }
    }
}
