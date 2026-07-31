using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Cart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCart();
        }
    }

    private int PwdCount
    {
        get { return ViewState["PwdCount"] == null ? 0 : (int)ViewState["PwdCount"]; }
        set { ViewState["PwdCount"] = value; }
    }

    private void BindCart()
    {
        var cart = Azure.CartManager.GetCart();

        if (cart.Count == 0)
        {
            pnlCart.Visible = false;
            pnlEmpty.Visible = true;
            return;
        }

        pnlEmpty.Visible = false;
        pnlCart.Visible = true;

        rptCartItems.DataSource = cart;
        rptCartItems.DataBind();

        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        var cart = Azure.CartManager.GetCart();

        decimal subtotal = Azure.CartManager.Subtotal();

        decimal discount = 0m;
        int remaining = PwdCount;

        foreach (var item in cart)
        {
            if (remaining <= 0)
                break;

            int eligibleQty = Math.Min(item.Quantity, remaining);

            discount += item.Price * eligibleQty * 0.20m;

            remaining -= eligibleQty;
        }

        decimal total = subtotal - discount;

        litSubtotal.Text = "&#8369;" + subtotal.ToString("0.00");
        litDiscountAmount.Text = "- &#8369;" + discount.ToString("0.00");
        litTotal.Text = "&#8369;" + total.ToString("0.00");
        litPwdCount.Text = PwdCount.ToString();
    }

    protected void btnPwdInc_Click(object sender, EventArgs e)
    {
        int totalItems = Azure.CartManager.GetCart().Sum(c => c.Quantity);

        if (PwdCount < totalItems)
            PwdCount++;

        RecalculateTotals();
    }

    protected void btnPwdDec_Click(object sender, EventArgs e)
    {
        if (PwdCount > 0)
            PwdCount--;

        RecalculateTotals();
    }

    protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int index = e.Item.ItemIndex;

        switch (e.CommandName)
        {
            case "Inc":
                Azure.CartManager.ChangeQuantity(index, 1);
                break;

            case "Dec":
                Azure.CartManager.ChangeQuantity(index, -1);
                break;

            case "Del":
                Azure.CartManager.RemoveAt(index);
                break;
        }

        BindCart();
    }

    protected void btnCheckout_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        var cart = Azure.CartManager.GetCart();

        if (cart.Count == 0)
            return;

        decimal subtotal = Azure.CartManager.Subtotal();

        decimal discount = 0m;
        int remaining = PwdCount;

        foreach (var item in cart)
        {
            if (remaining <= 0)
                break;

            int eligibleQty = Math.Min(item.Quantity, remaining);

            discount += item.Price * eligibleQty * 0.20m;

            remaining -= eligibleQty;
        }

        decimal total = subtotal - discount;

        int totalItems = cart.Sum(c => c.Quantity);
        int customerNo = Azure.OrderManager.GetNextCustomerNumber();

        Session["LastOrderNumber"] = customerNo.ToString();
        Session["LastOrderName"] = txtCustomerName.Text.Trim();
        Session["LastOrderSummary"] = string.Format(
            "{0} item(s) &middot; {1} &middot; Total: <strong>&#8369;{2}</strong> {3}",
            totalItems,
            ddlOrderType.SelectedValue,
            total.ToString("0.00"),
            discount > 0 ? "(PWD/Senior discount applied)" : "");

        Azure.CartManager.Clear();

        Response.Redirect("OrderConfirmation.aspx");
    }
}
