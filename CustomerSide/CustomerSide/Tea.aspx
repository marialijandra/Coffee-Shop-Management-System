<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Tea.aspx.cs" Inherits="Tea" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="section" style="padding-top:53px;">
        <div class="container">
            <div class="menu-head">
                <div class="filter-pills">
                    <a href="#" data-filter="all" class="active">All</a>
                    <a href="#" data-filter="Tea &amp; Cream">Tea &amp; Cream</a>
                    <a href="#" data-filter="Seasonal">Seasonal</a>
                </div>
                <div class="menu-title-block">
                    <h1 class="section-title">OUR TEA</h1>
                    <span class="section-sub">Every sip, a story</span>
                </div>
            </div>

            <div class="product-grid">
                <asp:Repeater ID="rptTea" runat="server" OnItemCommand="rptTea_ItemCommand">
                    <ItemTemplate>
                        <div class="product-card" data-tag='<%# Eval("Tag") %>'>
                            <div class="product-media">
                                <span class="badge"><%# Eval("Badge") %></span>
                                <button type="button" class="add-btn" onclick="toggleOptionsPopover(this)">+</button>
                                <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Name") %>' />
                            </div>

                            <div class="options-popover">
                                <div class="popover-name"><%# Eval("Name") %></div>
                                <div>
                                    <span class="option-label">Temperature</span>
                                    <div class="toggle-row">
                                        <button type="button" class="toggle-btn active" data-group="temp" data-value="Iced" onclick="selectOption(this)">Iced</button>
                                        <button type="button" class="toggle-btn" data-group="temp" data-value="Hot" onclick="selectOption(this)">Hot</button>
                                    </div>
                                </div>
                                <div>
                                    <span class="option-label">Size</span>
                                    <div class="toggle-row">
                                        <button type="button" class="toggle-btn active" data-group="size" data-value="S" onclick="selectOption(this)">Small</button>
                                        <button type="button" class="toggle-btn" data-group="size" data-value="L" onclick="selectOption(this)">Large</button>
                                    </div>
                                </div>
                                <div>
                                    <span class="option-label">Quantity</span>
                                    <div class="qty-row">
                                        <button type="button" class="qty-btn" onclick="changeQty(this,-1)">-</button>
                                        <span class="qty-value">1</span>
                                        <button type="button" class="qty-btn" onclick="changeQty(this,1)">+</button>
                                    </div>
                                </div>

                                <div class="popover-price"><%# Eval("FormattedPrice") %></div>

                                <input type="hidden" class="opt-temp" name='opt_temp_<%# Eval("Id") %>' value="Iced" />
                                <input type="hidden" class="opt-size" name='opt_size_<%# Eval("Id") %>' value="S" />
                                <input type="hidden" class="opt-qty" name='opt_qty_<%# Eval("Id") %>' value="1" />

                                <asp:LinkButton runat="server" CssClass="btn btn-primary popover-add"
                                    CommandName="AddToCart" CommandArgument='<%# Eval("Id") %>'>Add to Cart</asp:LinkButton>

                                <button type="button" class="popover-close" onclick="toggleOptionsPopover(this)">&times;</button>
                            </div>

                            <div class="product-name"><%# Eval("Name") %></div>
                            <p class="product-desc"><%# Eval("Description") %></p>
                            <div class="product-footer">
                                <span class="product-price"><%# Eval("FormattedPrice") %></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>

</asp:Content>
