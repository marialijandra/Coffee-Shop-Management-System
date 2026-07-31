using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CashierOrders : System.Web.UI.Page
{
    private string CurrentFilter
    {
        get { return ViewState["Filter"] as string ?? "All"; }
        set { ViewState["Filter"] = value; }
    }

    private string SearchTerm
    {
        get { return ViewState["Search"] as string ?? ""; }
        set { ViewState["Search"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindOrders();
        }
    }

    private void BindOrders()
    {
        var orders = Azure.OrderStore.GetAll();

        if (CurrentFilter != "All")
        {
            orders = orders.Where(o => o.Status == CurrentFilter).ToList();
        }

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            string term = SearchTerm.Trim().ToLower();
            orders = orders.Where(o =>
                (o.CustomerName != null && o.CustomerName.ToLower().Contains(term)) ||
                (o.RefCode != null && o.RefCode.ToLower().Contains(term)) ||
                o.Id.ToString().Contains(term)
            ).ToList();
        }

        rptOrders.DataSource = orders;
        rptOrders.DataBind();

        litNoOrders.Visible = orders.Count == 0;

        HighlightActiveFilter();

        txtSearch.Text = SearchTerm;
    }

    private void HighlightActiveFilter()
    {
        btnFilterAll.CssClass = CurrentFilter == "All" ? "active" : "";
        btnFilterNew.CssClass = CurrentFilter == "New Order" ? "active" : "";
        btnFilterProgress.CssClass = CurrentFilter == "In Progress" ? "active" : "";
        btnFilterServed.CssClass = CurrentFilter == "Served" ? "active" : "";
        btnFilterCancelled.CssClass = CurrentFilter == "Cancelled" ? "active" : "";
    }

    protected void FilterCommand(object sender, CommandEventArgs e)
    {
        CurrentFilter = e.CommandArgument.ToString();
        BindOrders();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchTerm = txtSearch.Text;
        BindOrders();
    }

    protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "SetStatus")
        {
            string[] parts = e.CommandArgument.ToString().Split('|');
            int orderId = Convert.ToInt32(parts[0]);
            Azure.OrderStore.TrySetStatus(orderId, parts[1]);
        }
        else if (e.CommandName == "SaveStatus")
        {
            int orderId = Convert.ToInt32(e.CommandArgument);
            string chosen = Request.Form["statusSelect_" + orderId];

            Azure.OrderStore.TrySetStatus(orderId, chosen);
        }
        else if (e.CommandName == "MarkPaid")
        {
            int orderId = Convert.ToInt32(e.CommandArgument);
            var ddl = (DropDownList)e.Item.FindControl("ddlPayMethod");
            string method = ddl != null ? ddl.SelectedValue : "Cash";

            int discountQty;
            int.TryParse(Request.Form["discountQty_" + orderId], out discountQty);

            Azure.OrderStore.MarkPaid(orderId, method, discountQty);
        }

        BindOrders();
    }
}
