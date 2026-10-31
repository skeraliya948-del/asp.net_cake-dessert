<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Admin_Dashboard.aspx.cs" Inherits="SweetDelights.Admin_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/admin-dashboard.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-shell">
        <aside class="admin-sidebar">
            <a href="Admin_Dashboard.aspx" class="admin-brand">
                <span class="admin-brand-icon"><i class="fa-solid fa-cake-candles"></i></span>
                <span>Sweet Delights<small>BAKERY ADMIN</small></span>
            </a>
            <p class="admin-menu-label">MAIN MENU</p>
            <a class="admin-nav active" href="Admin_Dashboard.aspx"><i class="fa-solid fa-chart-pie"></i> Dashboard</a>
            <a class="admin-nav" href="Admin_Orders.aspx"><i class="fa-solid fa-bag-shopping"></i> Manage Orders</a>
            <a class="admin-nav" href="#reports"><i class="fa-solid fa-chart-line"></i> Sales Reports</a>
            <a class="admin-nav" href="#recent"><i class="fa-solid fa-clock"></i> Recent Orders</a>
            <p class="admin-menu-label">CATALOG</p>
            <a class="admin-nav" href="Add_Products.aspx"><i class="fa-solid fa-cake-candles"></i> Add Product</a>
            <a class="admin-nav" href="Add_Company.aspx"><i class="fa-solid fa-layer-group"></i> Add Category</a>
            <a class="admin-nav" href="Show_Products.aspx"><i class="fa-solid fa-store"></i> View Store</a>
            <a class="admin-back" href="Default.aspx"><i class="fa-solid fa-arrow-left"></i> Back to website</a>
        </aside>

        <section class="admin-main">
            <header class="admin-page-header">
                <div>
                    <p class="admin-eyebrow">STORE MANAGEMENT</p>
                    <h1>Dashboard Overview</h1>
                    <p class="admin-subtitle">A fresh look at your bakery's performance.</p>
                </div>
                <div class="admin-date"><i class="fa-regular fa-calendar"></i>
                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                </div>
            </header>

            <div class="admin-welcome">
                <div>
                    <h2><asp:Label ID="lblAdminName" runat="server"></asp:Label></h2>
                    <p>Manage orders, products, and the Sweet Delights storefront from one place.</p>
                </div>
                <span class="welcome-cake"><i class="fa-solid fa-cookie-bite"></i></span>
            </div>

            <div class="admin-stats">
                <article class="admin-stat">
                    <div class="stat-top"><span>Paid Revenue</span><i class="fa-solid fa-indian-rupee-sign"></i></div>
                    <strong><asp:Label ID="lblRevenue" runat="server" Text="₹0.00"></asp:Label></strong>
                    <small>Revenue from paid orders</small>
                </article>
                <article class="admin-stat">
                    <div class="stat-top"><span>Total Orders</span><i class="fa-solid fa-bag-shopping"></i></div>
                    <strong><asp:Label ID="lblOrders" runat="server" Text="0"></asp:Label></strong>
                    <small>Orders placed by customers</small>
                </article>
                <article class="admin-stat">
                    <div class="stat-top"><span>Customers</span><i class="fa-solid fa-users"></i></div>
                    <strong><asp:Label ID="lblUsers" runat="server" Text="0"></asp:Label></strong>
                    <small>Registered customer accounts</small>
                </article>
                <article class="admin-stat">
                    <div class="stat-top"><span>Products</span><i class="fa-solid fa-box-open"></i></div>
                    <strong><asp:Label ID="lblProducts" runat="server" Text="0"></asp:Label></strong>
                    <small>Bakery products in the catalog</small>
                </article>
            </div>

            <div class="admin-content-grid" id="reports">
                <article class="admin-panel">
                    <div class="panel-heading">
                        <div><h2>Revenue Analytics</h2><p>Paid revenue for the last six months</p></div>
                        <span class="panel-icon"><i class="fa-solid fa-chart-column"></i></span>
                    </div>
                    <asp:Repeater ID="rptRevenue" runat="server">
                        <ItemTemplate>
                            <div class="revenue-row">
                                <span class="chart-month"><%#: Eval("Month") %></span>
                                <div class="chart-track">
                                    <div class="chart-fill" style='<%# "width:" + Eval("Percentage", "{0:0.##}") + "%;" %>'></div>
                                </div>
                                <strong class="chart-amount"><%# Eval("Revenue", "{0:C0}") %></strong>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </article>

                <article class="admin-panel">
                    <div class="panel-heading">
                        <div><h2>Order Status</h2><p>Distribution of customer orders</p></div>
                        <span class="panel-icon"><i class="fa-solid fa-chart-simple"></i></span>
                    </div>
                    <asp:Repeater ID="rptStatus" runat="server">
                        <ItemTemplate>
                            <div class="status-row">
                                <div class="status-heading">
                                    <span><%#: Eval("Status") %></span>
                                    <strong><%# Eval("Count") %></strong>
                                </div>
                                <div class="status-track">
                                    <div class="status-fill" style='<%# "width:" + Eval("Percentage", "{0:0.##}") + "%;" %>'></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </article>
            </div>

            <div class="admin-content-grid recent-grid" id="recent">
                <article class="admin-panel">
                    <div class="panel-heading">
                        <div><h2>Recent Orders</h2><p>Latest customer purchases</p></div>
                        <a class="panel-action" href="Admin_Orders.aspx">View all <i class="fa-solid fa-arrow-right"></i></a>
                    </div>
                    <div class="admin-table-wrap">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                            GridLines="None" CssClass="admin-table" EmptyDataText="No orders yet.">
                            <Columns>
                                <asp:HyperLinkField HeaderText="Order" DataTextField="Order_No"
                                    DataNavigateUrlFields="Order_No"
                                    DataNavigateUrlFormatString="Admin_Orders.aspx?orderno={0}" />
                                <asp:BoundField DataField="Name" HeaderText="Customer" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="Pay_Status" HeaderText="Payment" />
                                <asp:BoundField DataField="Order_Status" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </article>

                <article class="admin-panel quick-panel">
                    <div class="panel-heading"><div><h2>Quick Actions</h2><p>Common store tasks</p></div></div>
                    <a class="quick-action" href="Admin_Orders.aspx"><i class="fa-solid fa-bag-shopping"></i><span>Manage Orders</span><i class="fa-solid fa-chevron-right"></i></a>
                    <a class="quick-action" href="Add_Products.aspx"><i class="fa-solid fa-circle-plus"></i><span>Add a Product</span><i class="fa-solid fa-chevron-right"></i></a>
                    <a class="quick-action" href="Add_Company.aspx"><i class="fa-solid fa-layer-group"></i><span>Add a Category</span><i class="fa-solid fa-chevron-right"></i></a>
                    <div class="mini-stats">
                        <div><span>Pending Orders</span><strong><asp:Label ID="lblPending" runat="server" Text="0"></asp:Label></strong></div>
                        <div><span>Items in Orders</span><strong><asp:Label ID="lblItemsSold" runat="server" Text="0"></asp:Label></strong></div>
                    </div>
                </article>
            </div>

            <footer class="admin-footer">Sweet Delights Bakery &middot; Admin Dashboard</footer>
        </section>
    </div>
</asp:Content>
