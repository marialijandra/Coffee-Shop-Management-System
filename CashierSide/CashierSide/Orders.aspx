<%@ Page Title="" Language="C#" MasterPageFile="~/CashierSite.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="CashierOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="cashier-topbar">
        <div>
            <h1>Orders</h1>
            <div class="subtitle">Track each order from new to served, and take payment.</div>
        </div>
        <div class="today-date"><%= DateTime.Now.ToString("dddd, d MMMM yyyy") %></div>
    </div>

    <div class="orders-toolbar">
        <div class="order-filters">
            <asp:LinkButton ID="btnFilterAll" runat="server" CommandName="Filter" CommandArgument="All" OnCommand="FilterCommand">All</asp:LinkButton>
            <asp:LinkButton ID="btnFilterNew" runat="server" CommandName="Filter" CommandArgument="New Order" OnCommand="FilterCommand">New Order</asp:LinkButton>
            <asp:LinkButton ID="btnFilterProgress" runat="server" CommandName="Filter" CommandArgument="In Progress" OnCommand="FilterCommand">In Progress</asp:LinkButton>
            <asp:LinkButton ID="btnFilterServed" runat="server" CommandName="Filter" CommandArgument="Served" OnCommand="FilterCommand">Served</asp:LinkButton>
            <asp:LinkButton ID="btnFilterCancelled" runat="server" CommandName="Filter" CommandArgument="Cancelled" OnCommand="FilterCommand">Cancelled</asp:LinkButton>
        </div>

        <asp:Panel ID="pnlSearch" runat="server" CssClass="search-box" DefaultButton="btnSearch">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="7"/><path d="M21 21l-4.3-4.3" stroke-linecap="round"/></svg>
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search a name, order, or ref" />
            <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" style="display:none;">Search</asp:LinkButton>
        </asp:Panel>
    </div>

    <asp:Literal ID="litNoOrders" runat="server" Visible="false">
        <div class="empty-note">No orders match this view.</div>
    </asp:Literal>

    <div class="order-grid">
        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
            <ItemTemplate>
                <div class='<%# Eval("CardCssClass") %>'>
                    <div class="order-card-head">
                        <div class='<%# Eval("RefCssClass") %>'><%# Eval("RefCode") %></div>
                        <div class="order-head-info">
                            <div class="order-customer"><%# Eval("CustomerName") %></div>
                            <div class="order-meta">Order #<%# Eval("Id") %> &middot; <%# Eval("OrderType") %></div>
                        </div>
                        <span class='status-badge <%# Eval("StatusCssClass") %>'><span class="dot"></span><%# Eval("Status") %></span>
                    </div>

                    <div class="order-timestamp"><%# Eval("FormattedDate") %> &middot; <%# Eval("FormattedTime") %> &middot; <%# Eval("StatusSubLabel") %></div>

                    <table class="order-items-table">
                        <tr><th>Items</th><th class="num">Qty</th><th class="num">Price</th></tr>
                        <asp:Repeater ID="rptItems" runat="server" DataSource='<%# Eval("DisplayItems") %>'>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div class="oi-name"><%# Eval("Name") %></div>
                                        <div class="oi-meta"><%# Eval("OptionsLabel") %></div>
                                    </td>
                                    <td class="num"><%# Eval("Quantity") %></td>
                                    <td class="num"><%# Eval("FormattedLineTotal") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("HasMoreItems") %>'>
                            <tr class="order-more-row"><td colspan="3">+<%# Eval("MoreItemsCount") %> more item(s)</td></tr>
                        </asp:PlaceHolder>
                    </table>

                    <div class="order-total-row">
                        <span>Total</span>
                        <span><%# Eval("FormattedTotal") %></span>
                    </div>

                    <div class="order-actions">
                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("ShowDetailsButton") %>'>
                            <button type="button" class="btn btn-details" onclick="openDetailsModal(this)">See Details</button>
                        </asp:PlaceHolder>

                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("ShowPaymentButton") %>'>
                            <button type="button" class="btn btn-pay" onclick="openPayModal(this)">Payment</button>
                        </asp:PlaceHolder>
                    </div>

                    <div class="details-source" style="display:none;" data-status-class='<%# Eval("StatusCssClass") %>'>
                        <h3 class="modal-title"><%# Eval("CustomerName") %> &middot; <%# Eval("RefCode") %></h3>
                        <div class="modal-grid">
                            <div>
                                <span class="modal-section-label">Order #<%# Eval("Id") %> &middot; <%# Eval("OrderType") %></span>
                                <div class="receipt-list">
                                    <asp:Repeater runat="server" DataSource='<%# Eval("Items") %>'>
                                        <ItemTemplate>
                                            <div class="receipt-row">
                                                <div>
                                                    <div class="ri-name"><%# Eval("Name") %> &times; <%# Eval("Quantity") %></div>
                                                    <div class="ri-meta"><%# Eval("OptionsLabel") %></div>
                                                </div>
                                                <div><%# Eval("FormattedLineTotal") %></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <div class="receipt-summary">
                                    <div class="summary-row"><span>Subtotal</span><span><%# Eval("FormattedSubtotal") %></span></div>
                                    <div class="summary-row"><span>Discount</span><span><%# Eval("FormattedDiscount") %></span></div>
                                    <div class="summary-row total"><span>Total</span><span><%# Eval("FormattedTotal") %></span></div>
                                </div>
                            </div>
                            <div>
                                <span class="modal-section-label">Order Status</span>

                                <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("StatusLocked") %>'>
                                    <div class="status-stepper">
                                        <div class="status-step-btn current locked"><span class="dot"></span> <%# Eval("Status") %></div>
                                    </div>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder runat="server" Visible='<%# !(bool)Eval("StatusLocked") %>'>
                                    <div class="status-stepper">
                                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("ShowNewOrderStep") %>'>
                                            <button type="button" class='<%# (Eval("Status").ToString() == "New Order") ? "status-step-btn current" : "status-step-btn" %>'
                                                onclick="selectStatusStep(this, 'New Order')">
                                                <span class="dot"></span> New Order
                                            </button>
                                        </asp:PlaceHolder>
                                        <button type="button" class='<%# (Eval("Status").ToString() == "In Progress") ? "status-step-btn current" : "status-step-btn" %>'
                                            onclick="selectStatusStep(this, 'In Progress')">
                                            <span class="dot"></span> In Progress
                                        </button>
                                        <button type="button" class='<%# (Eval("Status").ToString() == "Served") ? "status-step-btn current" : "status-step-btn" %>'
                                            onclick="selectStatusStep(this, 'Served')">
                                            <span class="dot"></span> Served
                                        </button>
                                        <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("CanCancel") %>'>
                                            <button type="button" class='<%# (Eval("Status").ToString() == "Cancelled") ? "status-step-btn current" : "status-step-btn" %>'
                                                onclick="selectStatusStep(this, 'Cancelled')">
                                                <span class="dot"></span> Cancelled
                                            </button>
                                        </asp:PlaceHolder>
                                        <input type="hidden" class="status-select-hidden" name='statusSelect_<%# Eval("Id") %>' value='<%# Eval("Status") %>' />
                                    </div>

                                    <asp:LinkButton runat="server" CssClass="status-save-btn" CommandName="SaveStatus" CommandArgument='<%# Eval("Id") %>'>
                                        Save Status
                                    </asp:LinkButton>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>

                    <div class="pay-source"
                         style="display:none;"
                         data-subtotal-raw='<%# Eval("SubtotalRaw") %>'
                         data-total-raw='<%# Eval("TotalRaw") %>'
                         data-discount-requested='<%# Eval("DiscountRequested") %>'>
                        <h3 class="modal-title">Payment</h3>
                        <div class="modal-grid">
                            <div>
                                <span class="modal-section-label">Customer Info</span>
                                <div class="customer-info-row">
                                    <div class='<%# Eval("RefCssClass") %>'><%# Eval("RefCode") %></div>
                                    <div class="customer-info-name">
                                        <div class="order-customer"><%# Eval("CustomerName") %></div>
                                        <div class="order-meta">Order #<%# Eval("Id") %> &middot; <%# Eval("OrderType") %></div>
                                    </div>
                                    <div class="customer-info-datetime">
                                        <%# Eval("FormattedDate") %><br /><%# Eval("FormattedTime") %>
                                    </div>
                                </div>

                                <span class="modal-section-label">Transaction Details</span>
                                <div class="transaction-panel">
                                    <div class="receipt-list">
                                        <asp:Repeater runat="server" DataSource='<%# Eval("Items") %>'>
                                            <ItemTemplate>
                                                <div class="receipt-row">
                                                    <div>
                                                        <div class="ri-name"><%# Eval("Name") %> &times; <%# Eval("Quantity") %></div>
                                                        <div class="ri-meta"><%# Eval("OptionsLabel") %></div>
                                                    </div>
                                                    <div><%# Eval("FormattedLineTotal") %></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                    <div class="receipt-summary">
                                        <div class="summary-row"><span>Subtotal</span><span><%# Eval("FormattedSubtotal") %></span></div>
                                        <div class="summary-row"><span>Discount</span><span class="pay-discount-line"><%# Eval("FormattedDiscount") %></span></div>
                                        <div class="summary-row total"><span>Total</span><span class="pay-total-amount"><%# Eval("FormattedPayableTotal") %></span></div>
                                    </div>
                                </div>

                                <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("DiscountRequested") %>'>
                                    <label class="discount-check-row">
                                        <asp:CheckBox ID="chkValidateDiscount" runat="server" CssClass="discount-validate-checkbox" />
                                        PWD / Senior Citizen ID validated
                                        (<%# Eval("FormattedDiscount") %>)
                                    </label>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder runat="server" Visible='<%# (bool)Eval("CanCancel") %>'>
                                    <asp:LinkButton runat="server" CssClass="cancel-order-link"
                                        CommandName="SetStatus" CommandArgument='<%# Eval("Id") + "|Cancelled" %>'>
                                        Cancel this order instead
                                    </asp:LinkButton>
                                </asp:PlaceHolder>
                            </div>
                            <div>
                                <span class="modal-section-label">Select a payment method</span>
                                <asp:DropDownList ID="ddlPayMethod" runat="server" CssClass="pay-method-select">
                                    <asp:ListItem Text="Cash" Value="Cash" />
                                    <asp:ListItem Text="Card" Value="Card" />
                                </asp:DropDownList>

                                <div class="cash-panel">
                                    <div class="cash-amount-display">&#8369;<span class="cash-amount-value">0.00</span></div>

                                    <div class="quick-cash-row">
                                        <button type="button" class="quick-cash-btn" data-add="20">&#8369;20</button>
                                        <button type="button" class="quick-cash-btn" data-add="50">&#8369;50</button>
                                        <button type="button" class="quick-cash-btn" data-add="100">&#8369;100</button>
                                        <button type="button" class="quick-cash-btn" data-add="500">&#8369;500</button>
                                    </div>

                                    <div class="cash-keypad">
                                        <button type="button" class="key-btn" data-key="1">1</button>
                                        <button type="button" class="key-btn" data-key="2">2</button>
                                        <button type="button" class="key-btn" data-key="3">3</button>
                                        <button type="button" class="key-btn" data-key="4">4</button>
                                        <button type="button" class="key-btn" data-key="5">5</button>
                                        <button type="button" class="key-btn" data-key="6">6</button>
                                        <button type="button" class="key-btn" data-key="7">7</button>
                                        <button type="button" class="key-btn" data-key="8">8</button>
                                        <button type="button" class="key-btn" data-key="9">9</button>
                                        <button type="button" class="key-btn" data-key=".">.</button>
                                        <button type="button" class="key-btn" data-key="0">0</button>
                                        <button type="button" class="key-btn key-clear" data-key="clear">Clear</button>
                                    </div>

                                    <div class="change-row">
                                        <span>Change</span>
                                        <span class="change-amount">&#8369;0.00</span>
                                    </div>
                                </div>

                                <input type="hidden" class="cash-tendered-hidden" value="0" />

                                <asp:LinkButton runat="server" CssClass="pay-confirm-btn" CommandName="MarkPaid" CommandArgument='<%# Eval("Id") %>'>
                                    Pay Now
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div id="detailsModalOverlay" class="modal-overlay" onclick="closeModals(event)">
        <div class="modal-box" onclick="event.stopPropagation();">
            <button type="button" class="modal-close" onclick="closeModals()">&times;</button>
            <div id="detailsModalBody"></div>
        </div>
    </div>

    <div id="payModalOverlay" class="modal-overlay" onclick="closeModals(event)">
        <div class="modal-box" onclick="event.stopPropagation();">
            <button type="button" class="modal-close" onclick="closeModals()">&times;</button>
            <div id="payModalBody"></div>
        </div>
    </div>

    <script type="text/javascript">
        var STATUS_CLASSES = ['status-new', 'status-progress', 'status-served', 'status-cancelled'];

        function openDetailsModal(btn) {
            var card = btn.closest('.order-card');
            var source = card.querySelector('.details-source');
            var body = document.getElementById('detailsModalBody');
            body.innerHTML = '';
            source.style.display = 'block';
            body.appendChild(source);

            var overlay = document.getElementById('detailsModalOverlay');
            var modalBox = overlay.querySelector('.modal-box');
            modalBox.classList.remove.apply(modalBox.classList, STATUS_CLASSES);
            var statusClass = source.getAttribute('data-status-class');
            if (statusClass) modalBox.classList.add(statusClass);

            overlay.className = 'modal-overlay open';
        }

        function selectStatusStep(btn, value) {
            var stepper = btn.closest('.status-stepper');
            var buttons = stepper.querySelectorAll('.status-step-btn');
            for (var i = 0; i < buttons.length; i++) {
                buttons[i].classList.remove('current');
            }
            btn.classList.add('current');
            var hidden = stepper.querySelector('.status-select-hidden');
            if (hidden) hidden.value = value;
        }

        function openPayModal(btn) {
            var card = btn.closest('.order-card');
            var source = card.querySelector('.pay-source');
            var body = document.getElementById('payModalBody');
            body.innerHTML = '';
            source.style.display = 'block';
            body.appendChild(source);
            document.getElementById('payModalOverlay').className = 'modal-overlay open';
            initPayModal(source);
        }

        function closeModals(evt) {
            if (evt && evt.target !== evt.currentTarget) return;
            document.getElementById('detailsModalOverlay').className = 'modal-overlay';
            document.getElementById('payModalOverlay').className = 'modal-overlay';
        }

        function initPayModal(container) {
            var subtotal = parseFloat(container.getAttribute('data-subtotal-raw')) || 0;
            var discountedTotal = parseFloat(container.getAttribute('data-total-raw')) || 0;

            var methodSelect = container.querySelector('.pay-method-select');
            var cashPanel = container.querySelector('.cash-panel');
            var checkbox = container.querySelector('.discount-validate-checkbox');
            var totalLabel = container.querySelector('.pay-total-amount');
            var discountLine = container.querySelector('.pay-discount-line');
            var amountValueEl = container.querySelector('.cash-amount-value');
            var changeEl = container.querySelector('.change-amount');
            var tenderedHidden = container.querySelector('.cash-tendered-hidden');
            var keyButtons = container.querySelectorAll('.key-btn');
            var quickButtons = container.querySelectorAll('.quick-cash-btn');

            var amountText = '0';

            function currentPayable() {
                if (checkbox && checkbox.checked) return discountedTotal;
                return subtotal;
            }

            function refreshTotals() {
                var payable = currentPayable();
                if (totalLabel) totalLabel.innerHTML = '&#8369;' + payable.toFixed(2);
                if (discountLine) {
                    var discountAmt = subtotal - discountedTotal;
                    discountLine.innerHTML = (checkbox && checkbox.checked && discountAmt > 0)
                        ? '- &#8369;' + discountAmt.toFixed(2)
                        : '&#8369;0.00';
                }
                refreshChange();
            }

            function refreshChange() {
                var tendered = parseFloat(amountText) || 0;
                var payable = currentPayable();
                var change = tendered - payable;
                if (changeEl) changeEl.textContent = '\u20B1' + (change > 0 ? change.toFixed(2) : '0.00');
                if (tenderedHidden) tenderedHidden.value = tendered.toFixed(2);
            }

            function setAmount(text) {
                amountText = text;
                if (amountValueEl) amountValueEl.textContent = parseFloat(amountText || '0').toFixed(2);
                refreshChange();
            }

            if (methodSelect) {
                methodSelect.onchange = function () {
                    if (cashPanel) cashPanel.style.display = (methodSelect.value === 'Cash') ? '' : 'none';
                };
                if (cashPanel) cashPanel.style.display = (methodSelect.value === 'Cash') ? '' : 'none';
            }

            if (checkbox) {
                checkbox.onchange = refreshTotals;
            }

            keyButtons.forEach(function (key) {
                key.onclick = function () {
                    var k = key.getAttribute('data-key');
                    if (k === 'clear') {
                        setAmount('0');
                        return;
                    }
                    if (k === '.' && amountText.indexOf('.') !== -1) return;
                    if (amountText === '0' && k !== '.') {
                        setAmount(k);
                    } else {
                        setAmount(amountText + k);
                    }
                };
            });

            quickButtons.forEach(function (q) {
                q.onclick = function () {
                    var add = parseFloat(q.getAttribute('data-add')) || 0;
                    var current = parseFloat(amountText) || 0;
                    setAmount((current + add).toFixed(2));
                };
            });

            setAmount('0');
            refreshTotals();
        }
    </script>

</asp:Content>
