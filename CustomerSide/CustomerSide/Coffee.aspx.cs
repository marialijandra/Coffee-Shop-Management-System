using System;

public partial class Coffee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCoffee();
        }
    }

    private void BindCoffee()
    {
        rptCoffee.DataSource = Azure.ProductCatalog.GetByCategory("Coffee");
        rptCoffee.DataBind();
    }

    protected void rptCoffee_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "AddToCart") return;

        int productId = Convert.ToInt32(e.CommandArgument);
        var product = Azure.ProductCatalog.GetById(productId);
        if (product == null) return;

        if (product.IsSoldOut)
        {
            BindCoffee();
            return;
        }

        string temperature = Request.Form["opt_temp_" + productId] ?? "Iced";
        string size = Request.Form["opt_size_" + productId] ?? "S";
        int qty;
        if (!int.TryParse(Request.Form["opt_qty_" + productId], out qty) || qty < 1) qty = 1;

        Azure.CartManager.AddItem(product, qty, temperature, size);

        var master = (SiteMaster)this.Master;
        master.ShowAddedToCartToast(product.Name, qty);

        BindCoffee();
    }
}
