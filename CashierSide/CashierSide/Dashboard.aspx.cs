using System;
using System.Collections.Generic;
using System.Linq;

public partial class CashierDashboard : System.Web.UI.Page
{
    private List<Azure.Order> _today;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _today = Azure.OrderStore.GetForDate(DateTime.Today);

            BindStats();
            BindTransactions();
        }
    }

    private void BindStats()
    {
        var counted = _today.Where(o => o.Status != Azure.Order.StatusCancelled).ToList();

        decimal revenue = counted.Where(o => o.IsPaid).Sum(o => o.Total);
        int ordersToday = counted.Count;
        int itemsToday = counted.Sum(o => o.ItemCount);
        int paidToday = counted.Count(o => o.IsPaid);

        litRevenue.Text = Azure.Pricing.Peso(revenue);
        litRevenueSub.Text = paidToday + " paid order(s) so far";

        litOrdersToday.Text = ordersToday.ToString();
        litOrdersSub.Text = (ordersToday - paidToday) + " awaiting payment";

        litItemsToday.Text = itemsToday.ToString();
    }

    private void BindTransactions()
    {
        var paid = _today
            .Where(o => o.IsPaid && o.Status != Azure.Order.StatusCancelled)
            .OrderByDescending(o => o.CreatedAt)
            .Take(20)
            .Select(o => new TransactionRow(o))
            .ToList();

        rptTransactions.DataSource = paid;
        rptTransactions.DataBind();

        litNoTransactions.Visible = paid.Count == 0;
    }

    public class TransactionRow
    {
        public TransactionRow(Azure.Order order)
        {
            OrderNumberLabel = "Order #" + order.ShortNumber;
            CustomerName = order.CustomerName;
            RefCssClass = order.RefCssClass;
            RefCode = order.RefCode;
            ShortNumber = order.ShortNumber;
            FormattedDate = order.FormattedDate;
            FormattedTime = order.FormattedTime;
            FormattedTotal = order.FormattedAmountPaid;
            PaymentMethod = order.PaymentMethod;
            MethodCssClass = "method-" + (order.PaymentMethod ?? "cash").ToLower();
        }

        public string OrderNumberLabel { get; set; }
        public string CustomerName { get; set; }
        public string RefCssClass { get; set; }
        public string RefCode { get; set; }
        public string ShortNumber { get; set; }
        public string FormattedDate { get; set; }
        public string FormattedTime { get; set; }
        public string FormattedTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string MethodCssClass { get; set; }
    }
}
