<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OrderConfirmation.aspx.cs" Inherits="OrderConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="section" style="padding-top:60px;">
        <div class="container" style="max-width:520px;">
            <div class="confirm-box">
                <h2>Order Placed, <asp:Literal ID="litConfirmName" runat="server" />!</h2>
                <p style="color:var(--dark-gray);">Your Customer No. is</p>
                <div class="customer-no-badge">#<asp:Literal ID="litCustomerNo" runat="server" /></div>
                <p style="max-width:420px; margin: 16px auto; font-size:14px; color:var(--dark-gray);">
                    Please proceed to the counter to pay for this order.<br />
                    <asp:Literal ID="litConfirmSummary" runat="server" />
                </p>
                <a href="Default.aspx" class="btn btn-brown">Order Something Else</a>
            </div>
        </div>
    </section>

</asp:Content>
