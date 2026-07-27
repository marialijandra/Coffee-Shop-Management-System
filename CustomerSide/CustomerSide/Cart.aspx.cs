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

    private decimal DiscountRate
    {
        get { return chkDiscount.Checked ? 0.20m : 0m; }
    }

    private void BindCart()
    {
        var cart = Azure.CartManager.GetCart();

        if (cart.Count == 0)
        {
            pnlCart.Visible = false;
            pnlEmpty.Visible = true;
            pnlConfirmation.Visible = false;
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
        decimal subtotal = Azure.CartManager.Subtotal();
        decimal discount = subtotal * DiscountRate;
        decimal total = subtotal - discount;

        litSubtotal.Text = "&#8369;" + subtotal.ToString("0.00");
        litDiscountAmount.Text = discount > 0 ? "- &#8369;" + discount.ToString("0.00") : "&#8369;0.00";
        litTotal.Text = "&#8369;" + total.ToString("0.00");
    }

    protected void chkDiscount_CheckedChanged(object sender, EventArgs e)
    {
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
        if (!Page.IsValid) return;

        var cart = Azure.CartManager.GetCart();
        if (cart.Count == 0) return;

        decimal subtotal = Azure.CartManager.Subtotal();
        decimal discount = subtotal * DiscountRate;
        decimal total = subtotal - discount;
        int totalItems = cart.Sum(c => c.Quantity);
        int customerNo = Azure.OrderManager.GetNextCustomerNumber();

        litConfirmName.Text = txtCustomerName.Text.Trim();
        litCustomerNo.Text = customerNo.ToString();
        litConfirmSummary.Text = string.Format(
            "{0} item(s) &middot; {1} &middot; Total: <strong>&#8369;{2}</strong> {3}",
            totalItems,
            ddlOrderType.SelectedValue,
            total.ToString("0.00"),
            DiscountRate > 0 ? "(PWD/Senior discount applied)" : "");

        Azure.CartManager.Clear();

        pnlCart.Visible = false;
        pnlEmpty.Visible = false;
        pnlConfirmation.Visible = true;
    }
}
