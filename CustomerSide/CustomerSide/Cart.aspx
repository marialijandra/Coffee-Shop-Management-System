<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header">
        <h1>Your Cart</h1>
        <p>Review your order before checking out.</p>
    </div>

    <section class="section" style="padding-top:20px;">
        <div class="container">

            <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
                <div class="confirm-box">
                    <h2>Order Placed, <asp:Literal ID="litConfirmName" runat="server" />!</h2>
                    <p style="color:var(--dark-gray);">Your Customer No. is</p>
                    <div class="customer-no-badge">#<asp:Literal ID="litCustomerNo" runat="server" /></div>
                    <p style="max-width:420px; margin: 16px auto; font-size:14px; color:var(--dark-gray);">
                        Please proceed to the cashier to pay for this order.<br />
                        <asp:Literal ID="litConfirmSummary" runat="server" />
                    </p>
                    <a href="Default.aspx#menu" class="btn btn-primary">Order Something Else</a>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
                <div class="empty-cart">
                    <h2>Your cart is empty</h2>
                    <p>Looks like you haven't added anything yet.</p>
                    <a href="Default.aspx#menu" class="btn btn-primary">Browse the Menu</a>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCart" runat="server">
                <div class="cart-page-grid">

                    <div>
                        <table class="cart-table">
                            <tr>
                                <th>Item</th>
                                <th>Price</th>
                                <th>Qty</th>
                                <th>Subtotal</th>
                                <th></th>
                            </tr>
                            <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="cart-row-product">
                                                <img src='<%# Eval("ImageUrl") %>' alt="" />
                                                <div>
                                                    <div><%# Eval("Name") %></div>
                                                    <div class="cart-row-meta"><%# Eval("OptionsLabel") %></div>
                                                </div>
                                            </div>
                                        </td>
                                        <td><%# Eval("FormattedPrice") %></td>
                                        <td>
                                            <div class="qty-stepper">
                                                <asp:LinkButton runat="server" CssClass="ci-qty-btn" CommandName="Dec">-</asp:LinkButton>
                                                <span><%# Eval("Quantity") %></span>
                                                <asp:LinkButton runat="server" CssClass="ci-qty-btn" CommandName="Inc">+</asp:LinkButton>
                                            </div>
                                        </td>
                                        <td><%# Eval("FormattedLineTotal") %></td>
                                        <td>
                                            <asp:LinkButton runat="server" CssClass="link-remove" CommandName="Del">Delete</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                    <div class="summary-card">
                        <h3>Order Summary</h3>

                        <div class="summary-row">
                            <span>Subtotal</span>
                            <span><asp:Literal ID="litSubtotal" runat="server" /></span>
                        </div>
                        <div class="summary-row">
                            <span>Discount</span>
                            <span><asp:Literal ID="litDiscountAmount" runat="server" /></span>
                        </div>
                        <div class="summary-row total">
                            <span>Total</span>
                            <span><asp:Literal ID="litTotal" runat="server" /></span>
                        </div>

                        <hr style="border:none; border-top:1px solid var(--gray); margin:18px 0;" />

                        <div class="form-field">
                            <label>Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" placeholder="Juan Dela Cruz" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCustomerName"
                                ErrorMessage="Please enter your name" Display="Dynamic" ForeColor="#b3402f" Font-Size="11px"
                                ValidationGroup="checkout" />
                        </div>

                        <div class="form-field">
                            <label>Order Type</label>
                            <asp:DropDownList ID="ddlOrderType" runat="server">
                                <asp:ListItem Text="Dine In" Value="Dine In" />
                                <asp:ListItem Text="Takeout" Value="Takeout" />
                            </asp:DropDownList>
                        </div>

                        <div class="form-field">
                            <label class="checkbox-field">
                                <asp:CheckBox ID="chkDiscount" runat="server" AutoPostBack="true" OnCheckedChanged="chkDiscount_CheckedChanged" />
                                PWD / Senior Citizen Discount (20% off)
                            </label>
                        </div>

                        <asp:Button ID="btnCheckout" runat="server" CssClass="btn btn-primary" style="width:100%; margin-top:10px;"
                            Text="Place Order" OnClick="btnCheckout_Click" ValidationGroup="checkout" />
                    </div>
                </div>
            </asp:Panel>

        </div>
    </section>

</asp:Content>
