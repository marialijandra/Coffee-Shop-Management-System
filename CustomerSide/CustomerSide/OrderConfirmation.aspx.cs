using System;

public partial class OrderConfirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LastOrderNumber"] == null)
        {
            Response.Redirect("Default.aspx");
            return;
        }

        litCustomerNo.Text = Session["LastOrderNumber"].ToString();
        litConfirmName.Text = Session["LastOrderName"] as string;
        litConfirmSummary.Text = Session["LastOrderSummary"] as string;

        Session.Remove("LastOrderNumber");
        Session.Remove("LastOrderName");
        Session.Remove("LastOrderSummary");
    }
}
