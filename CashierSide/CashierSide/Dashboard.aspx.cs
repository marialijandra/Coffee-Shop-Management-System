using System;
using System.Collections.Generic;
using System.Linq;

public partial class CashierDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindStats();
            BindTransactions();
        }
    }

    private void BindStats()
    {
        var all = Azure.OrderStore.GetAll();
        var today = all.Where(o => o.CreatedAt.Date == DateTime.Now.Date).ToList();

        decimal revenue = today.Where(o => o.IsPaid).Sum(o => o.Total);
        int ordersToday = today.Count;
        int itemsToday = today.Sum(o => o.ItemCount);
        int paidToday = today.Count(o => o.IsPaid);

        litRevenue.Text = "&#8369;" + revenue.ToString("0.00");
        litRevenueSub.Text = paidToday + " paid order(s) so far";

        litOrdersToday.Text = ordersToday.ToString();
        litOrdersSub.Text = (ordersToday - paidToday) + " awaiting payment";

        litItemsToday.Text = itemsToday.ToString();
    }

    private void BindTransactions()
    {
        var paid = Azure.OrderStore.GetAll()
            .Where(o => o.IsPaid)
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
            OrderNumberLabel = "Order #" + order.Id;
            CustomerName = order.CustomerName;
            RefCssClass = order.RefCssClass;
            RefCode = order.RefCode;
            FormattedDate = order.FormattedDate;
            FormattedTime = order.FormattedTime;
            FormattedTotal = order.FormattedTotal;
            PaymentMethod = order.PaymentMethod;
            MethodCssClass = "method-" + (order.PaymentMethod ?? "cash").ToLower();
        }

        public string OrderNumberLabel { get; set; }
        public string CustomerName { get; set; }
        public string RefCssClass { get; set; }
        public string RefCode { get; set; }
        public string FormattedDate { get; set; }
        public string FormattedTime { get; set; }
        public string FormattedTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string MethodCssClass { get; set; }
    }
}
