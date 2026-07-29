<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="section" style="padding-top:15px; padding-bottom:20px;">
        <div class="container">

            <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
                <div class="empty-cart">
                    <h2>Your cart is empty</h2>
                    <p>Looks like you haven't added anything yet.</p>
                    <a href="Default.aspx#menu" class="btn btn-primary">Browse the Menu</a>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCart" runat="server">
                <div class="cart-page-grid">

                    <div class="cart-items-scroll">
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
                                            <asp:LinkButton runat="server" CssClass="link-remove" CommandName="Del" ToolTip="Remove"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6l-1 14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2L5 6"></path><path d="M10 11v6"></path><path d="M14 11v6"></path><path d="M9 6V4a1 1 0 0 1 1-1h4a1 1 0 0 1 1 1v2"></path></svg></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                    <div class="cart-summary-column">
                        <div class="menu-title-block" style="text-align:center; margin-bottom:18px;">
                            <h1 class="section-title">YOUR CART</h1>
                            <span class="section-sub">Review your order before checking out.</span>
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
                            <div class="checkbox-field pwd-field">
                                <span>PWD / Senior Citizen Discount</span>
                                <div class="qty-stepper">
                                    <asp:LinkButton ID="btnPwdDec" runat="server" CssClass="ci-qty-btn" OnClick="btnPwdDec_Click">-</asp:LinkButton>
                                    <span><asp:Literal ID="litPwdCount" runat="server" Text="0" /></span>
                                    <asp:LinkButton ID="btnPwdInc" runat="server" CssClass="ci-qty-btn" OnClick="btnPwdInc_Click">+</asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <asp:Button ID="btnCheckout" runat="server" CssClass="btn btn-primary" style="width:100%; margin-top:10px;"
                            Text="Place Order" OnClick="btnCheckout_Click" ValidationGroup="checkout" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </div>
    </section>

</asp:Content>
