<%@ Page Title="" Language="C#" MasterPageFile="~/CashierSite.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="CashierMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="cashier-topbar">
        <div>
            <h1>Menu</h1>
            <div class="subtitle">Mark an item sold out and it disappears from the customer menu instantly.</div>
        </div>
        <div class="today-date"><%= DateTime.Now.ToString("dddd, d MMMM yyyy") %></div>
    </div>

    <div class="menu-grid">
        <asp:Repeater ID="rptMenu" runat="server">
            <ItemTemplate>
                <div class='menu-card<%# (bool)Eval("IsSoldOut") ? " sold-out" : "" %>'>
                    <div class="menu-card-media" style='background-image:url(<%# ResolveUrl("~/" + Eval("ImageUrl")) %>);'>
                        <div class="menu-media-backdrop"></div>
                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("IsSoldOut") %>'>
                            <div class="sold-out-flag">Sold Out</div>
                        </asp:PlaceHolder>
                        <img src='<%# ResolveUrl("~/" + Eval("ImageUrl")) %>' alt='<%# Eval("Name") %>' />
                    </div>

                    <div class="menu-card-tag"><%# Eval("Category") %> &middot; <%# Eval("Tag") %></div>
                    <div class="menu-card-name"><%# Eval("Name") %></div>

                    <div class="menu-toggle-row">
                        <span class="menu-toggle-label">Mark as sold out</span>
                        <label class="switch">
                            <asp:CheckBox ID="chkSoldOut" runat="server" AutoPostBack="true"
                                OnCheckedChanged="chkSoldOut_CheckedChanged"
                                Checked='<%# (bool)Eval("IsSoldOut") %>' />
                            <span class="slider"></span>
                        </label>
                        <asp:HiddenField ID="hidProductId" runat="server" Value='<%# Eval("Id") %>' />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
