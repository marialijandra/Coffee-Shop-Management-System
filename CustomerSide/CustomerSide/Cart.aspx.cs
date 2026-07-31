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

    private int DrinkCount
    {
        get { return Azure.CartManager.GetCart().Sum(c => c.Quantity); }
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

    private decimal CurrentDiscount()
    {
        return Azure.Pricing.DiscountFor(Azure.CartManager.GetCart(), PwdCount);
    }

    private void RecalculateTotals()
    {
        if (PwdCount > DrinkCount)
            PwdCount = DrinkCount;

        decimal subtotal = Azure.CartManager.Subtotal();
        decimal discount = CurrentDiscount();
        decimal total = subtotal - discount;

        litSubtotal.Text = Azure.Pricing.Peso(subtotal);
        litDiscountAmount.Text = discount > 0
            ? "- " + Azure.Pricing.Peso(discount)
            : Azure.Pricing.Peso(0m);
        litTotal.Text = Azure.Pricing.Peso(total);
        litPwdCount.Text = PwdCount.ToString();
    }

    protected void btnPwdInc_Click(object sender, EventArgs e)
    {
        if (PwdCount < DrinkCount)
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

        if (PwdCount > DrinkCount)
            PwdCount = DrinkCount;

        decimal subtotal = Azure.CartManager.Subtotal();
        decimal discount = CurrentDiscount();
        decimal total = subtotal - discount;

        int totalItems = cart.Sum(c => c.Quantity);
        int customerNo = Azure.OrderManager.GetNextCustomerNumber();

        Session["LastOrderNumber"] = customerNo.ToString();
        Session["LastOrderName"] = txtCustomerName.Text.Trim();
        Session["LastOrderSummary"] = string.Format(
            "{0} item(s) &middot; {1} &middot; Total: <strong>{2}</strong> {3}",
            totalItems,
            ddlOrderType.SelectedValue,
            Azure.Pricing.Peso(total),
            discount > 0 ? "(PWD/Senior discount applied)" : "");

        Azure.OrderRepository repo = new Azure.OrderRepository();

        string orderNumber = customerNo.ToString();

        repo.SaveOrder(
            orderNumber,
            txtCustomerName.Text.Trim(),
            ddlOrderType.SelectedValue,
            subtotal,
            discount,
            PwdCount,
            total,
            cart
        );

        Azure.CartManager.Clear();

        Response.Redirect("OrderConfirmation.aspx");
    }
}
