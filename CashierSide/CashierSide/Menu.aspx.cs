using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CashierMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMenu();
        }
    }

    private void BindMenu()
    {
        rptMenu.DataSource = Azure.ProductCatalog.GetAll();
        rptMenu.DataBind();
    }

    protected void chkSoldOut_CheckedChanged(object sender, EventArgs e)
    {
        var chk = (CheckBox)sender;
        var container = (RepeaterItem)chk.NamingContainer;
        var hidProductId = (HiddenField)container.FindControl("hidProductId");

        int productId = Convert.ToInt32(hidProductId.Value);
        Azure.ProductCatalog.SetSoldOut(productId, chk.Checked);

        BindMenu();
    }
}
