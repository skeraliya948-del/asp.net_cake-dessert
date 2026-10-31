<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Admin_Orders.aspx.cs" Inherits="SweetDelights.Admin_Orders" %>

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
            <a class="admin-nav" href="Admin_Dashboard.aspx"><i class="fa-solid fa-chart-pie"></i> Dashboard</a>
            <a class="admin-nav active" href="Admin_Orders.aspx"><i class="fa-solid fa-bag-shopping"></i> Manage Orders</a>
            <a class="admin-nav" href="Admin_Dashboard.aspx#reports"><i class="fa-solid fa-chart-line"></i> Sales Reports</a>
            <a class="admin-nav" href="Admin_Dashboard.aspx#recent"><i class="fa-solid fa-clock"></i> Recent Orders</a>
            <p class="admin-menu-label">CATALOG</p>
            <a class="admin-nav" href="Add_Products.aspx"><i class="fa-solid fa-cake-candles"></i> Add Product</a>
            <a class="admin-nav" href="Add_Company.aspx"><i class="fa-solid fa-layer-group"></i> Add Category</a>
            <a class="admin-back" href="Default.aspx"><i class="fa-solid fa-arrow-left"></i> Back to website</a>
        </aside>

        <section class="admin-main">
            <header class="admin-page-header">
                <div>
                    <p class="admin-eyebrow">STORE MANAGEMENT</p>
                    <h1>Manage Orders</h1>
                    <p class="admin-subtitle">Review customer purchases and update order or payment status.</p>
                </div>
                <a class="panel-action" href="Admin_Dashboard.aspx"><i class="fa-solid fa-arrow-left"></i> Dashboard</a>
            </header>

            <asp:Label ID="lblOrdersMessage" runat="server" CssClass="block text-sm font-bold text-rose-700 my-4"></asp:Label>

            <article class="admin-panel">
                <div class="panel-heading">
                    <div><h2>Customer Orders</h2><p>Save status changes for each order below.</p></div>
                </div>
                <div class="admin-table-wrap">
                    <asp:GridView ID="GridViewOrders" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="Order_No" GridLines="None" CssClass="admin-table"
                        EmptyDataText="No orders have been placed yet."
                        OnRowDataBound="GridViewOrders_RowDataBound"
                        OnRowCommand="GridViewOrders_RowCommand">
                        <Columns>
                            <asp:HyperLinkField HeaderText="Order" DataTextField="Order_No"
                                DataNavigateUrlFields="Order_No"
                                DataNavigateUrlFormatString="Admin_Orders.aspx?orderno={0}" />
                            <asp:BoundField DataField="Order_Date" HeaderText="Placed"
                                DataFormatString="{0:dd MMM yyyy, HH:mm}" />
                            <asp:BoundField DataField="Name" HeaderText="Customer" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Grand_Total" HeaderText="Amount"
                                DataFormatString="{0:C2}" />
                            <asp:TemplateField HeaderText="Order Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="order-status-select">
                                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                        <asp:ListItem Value="Processing">Processing</asp:ListItem>
                                        <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                        <asp:ListItem Value="Cancelled">Cancelled</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPayStatus" runat="server" CssClass="order-status-select">
                                        <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                        <asp:ListItem Value="Paid">Paid</asp:ListItem>
                                        <asp:ListItem Value="Refunded">Refunded</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSaveStatus" runat="server" Text="Save"
                                        CommandName="save_status" CommandArgument='<%# Eval("Order_No") %>'
                                        CausesValidation="false" CssClass="order-save-button"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </article>

            <asp:Panel ID="pnlOrderDetails" runat="server" Visible="false" CssClass="admin-panel mt-5">
                <div class="panel-heading">
                    <div><h2>Order Items</h2><p>Products saved with their order-time prices.</p></div>
                </div>
                <div class="admin-table-wrap">
                    <asp:GridView ID="GridViewDetails" runat="server" AutoGenerateColumns="False"
                        GridLines="None" CssClass="admin-table" EmptyDataText="No order items found.">
                        <Columns>
                            <asp:BoundField DataField="Product_Name" HeaderText="Product" />
                            <asp:BoundField DataField="Quentity" HeaderText="Quantity" />
                            <asp:BoundField DataField="Unit_Price" HeaderText="Unit Price" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Line_Total" HeaderText="Line Total" DataFormatString="{0:C2}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </section>
    </div>
</asp:Content>
