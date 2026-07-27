using System;
using System.IO;

public partial class CashierSiteMaster : System.Web.UI.MasterPage
{
    private string _currentPage;

    protected void Page_Load(object sender, EventArgs e)
    {
        _currentPage = Path.GetFileName(Request.Path);
    }

    protected string ActiveClass(string pageName)
    {
        return string.Equals(_currentPage, pageName, StringComparison.OrdinalIgnoreCase) ? "active" : "";
    }
}
