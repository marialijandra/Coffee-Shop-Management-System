using System;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindCartWidgets();
        HideFooterOnLandingPage();
        HighlightActiveNav();
    }

    private void HighlightActiveNav()
    {
        string page = System.IO.Path.GetFileName(Request.Path).ToLower();

        if (page == "default.aspx" || page == "")
            navSignature.Attributes["class"] = "active";
        else if (page == "coffee.aspx")
            navCoffee.Attributes["class"] = "active";
        else if (page == "tea.aspx")
            navTea.Attributes["class"] = "active";
    }

    private void HideFooterOnLandingPage()
    {
        string page = System.IO.Path.GetFileName(Request.Path).ToLower();
        if (page == "default.aspx" || page == "")
        {
            pnlFooter.Visible = false;
            pageBody.Attributes["class"] = "no-scroll";
        }
    }

    private void BindCartWidgets()
    {
        var cart = Azure.CartManager.GetCart();
        litCartCount.Text = Azure.CartManager.ItemCount().ToString();

        rptDrawerItems.DataSource = cart;
        rptDrawerItems.DataBind();

        litDrawerEmpty.Text = cart.Count == 0
            ? "<p style='color:#545555; font-size:13px; margin-top:20px;'>Your cart is empty. Go add something delicious!</p>"
            : "";
    }

    protected void rptDrawerItems_ItemCommand(object source, RepeaterCommandEventArgs e)
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

        BindCartWidgets();
    }

    public void ShowAddedToCartToast(string productName, int quantity)
    {
        pnlToast.Visible = true;
        litToastMessage.Text = quantity + " &times; " + productName + " added to your cart.";
        BindCartWidgets();
    }

    protected void btnDismissToast_Click(object sender, EventArgs e)
    {
        pnlToast.Visible = false;
    }
}
