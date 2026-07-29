<%@ Page Title="" Language="C#" MasterPageFile="~/CashierSite.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="CashierDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="cashier-topbar">
        <div>
            <h1>Dashboard</h1>
            <div class="subtitle">Today's performance at a glance.</div>
        </div>
        <div class="today-date"><%= DateTime.Now.ToString("dddd, d MMMM yyyy") %></div>
    </div>

    <div class="stat-grid">
        <div class="stat-card">
            <div class="stat-label">Today's Revenue</div>
            <div class="stat-value"><asp:Literal ID="litRevenue" runat="server" /></div>
            <div class="stat-sub"><asp:Literal ID="litRevenueSub" runat="server" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Orders Today</div>
            <div class="stat-value"><asp:Literal ID="litOrdersToday" runat="server" /></div>
            <div class="stat-sub"><asp:Literal ID="litOrdersSub" runat="server" /></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Items Sold Today</div>
            <div class="stat-value"><asp:Literal ID="litItemsToday" runat="server" /></div>
            <div class="stat-sub">Across all of today's orders</div>
        </div>
    </div>

    <div class="panel">
        <div class="panel-head">
            <h3>Recent Transactions</h3>
        </div>

        <div class="tx-scroll">
            <table class="tx-table">
                <tr>
                    <th>Order</th>
                    <th>Date</th>
                    <th>Amount</th>
                    <th>Method</th>
                </tr>
                <asp:Repeater ID="rptTransactions" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tx-product">
                                    <div class='<%# Eval("RefCssClass") %> tx-ref'><%# Eval("ShortNumber") %></div>
                                    <div>
                                        <div><%# Eval("OrderNumberLabel") %></div>
                                        <div class="tx-more"><%# Eval("CustomerName") %></div>
                                    </div>
                                </div>
                            </td>
                            <td><%# Eval("FormattedDate") %> &middot; <%# Eval("FormattedTime") %></td>
                            <td><%# Eval("FormattedTotal") %></td>
                            <td><span class='method-pill <%# Eval("MethodCssClass") %>'><%# Eval("PaymentMethod") %></span></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>

        <asp:Literal ID="litNoTransactions" runat="server" Visible="false">
            <div class="empty-note">No paid transactions yet today.</div>
        </asp:Literal>
    </div>

</asp:Content>
