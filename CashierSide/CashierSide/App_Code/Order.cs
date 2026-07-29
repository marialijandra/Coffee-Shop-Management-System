using System;
using System.Collections.Generic;
using System.Linq;

namespace Azure
{
    [Serializable]
    public class Order
    {
        public const string StatusNew = "New Order";
        public const string StatusInProgress = "In Progress";
        public const string StatusServed = "Served";
        public const string StatusCancelled = "Cancelled";

        public int Id { get; set; }
        public string RefCode { get; set; }
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DiscountQuantity { get; set; }

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

        public string FormattedSubtotal { get { return "&#8369;" + Subtotal.ToString("0.00"); } }
        public string FormattedDiscount { get { return Discount > 0 ? "- &#8369;" + Discount.ToString("0.00") : "&#8369;0.00"; } }
        public string FormattedTotal { get { return "&#8369;" + Total.ToString("0.00"); } }
        public string FormattedDate { get { return CreatedAt.ToString("MMM d, yyyy"); } }
        public string FormattedTime { get { return CreatedAt.ToString("hh:mm tt"); } }

        public bool DiscountRequested { get { return Discount > 0; } }

        public decimal AppliedDiscount
        {
            get { return DiscountRequested ? Math.Min(Discount * DiscountQuantity, Subtotal) : 0m; }
        }

        public decimal PayableTotal { get { return Subtotal - AppliedDiscount; } }

        public string FormattedPayableTotal { get { return "&#8369;" + PayableTotal.ToString("0.00"); } }
        public string FormattedAppliedDiscount { get { return AppliedDiscount > 0 ? "- &#8369;" + AppliedDiscount.ToString("0.00") : "&#8369;0.00"; } }

        public string SubtotalRaw { get { return Subtotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }
        public string TotalRaw { get { return Total.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }
        public string DiscountRaw { get { return Discount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture); } }

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
