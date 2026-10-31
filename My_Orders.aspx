<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="My_Orders.aspx.cs" Inherits="SweetDelights.My_Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-12 bg-rose-50/40 min-h-screen">
        <div class="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
            <div class="bg-gradient-to-r from-rose-600 via-pink-600 to-amber-600 text-white rounded-3xl p-6 sm:p-8 shadow-xl">
                <span class="text-xs uppercase tracking-wider text-rose-100 font-semibold">Sweet Delights</span>
                <h1 class="font-serif-heading text-3xl font-bold">Checkout</h1>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="block text-sm font-semibold text-rose-700"></asp:Label>

            <asp:Panel ID="pnlCheckout" runat="server" CssClass="bg-white rounded-3xl p-6 sm:p-8 border border-rose-100 shadow-xl space-y-7">
                <div>
                    <h2 class="font-serif-heading text-xl font-bold text-slate-900 mb-4">Customer details</h2>
                    <div class="grid md:grid-cols-2 gap-4">
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Name</label>
                            <asp:TextBox ID="txtcustname" runat="server" ReadOnly="true"
                                CssClass="w-full bg-slate-100 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-700"></asp:TextBox>
                        </div>
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Email</label>
                            <asp:TextBox ID="txtemail" runat="server" ReadOnly="true"
                                CssClass="w-full bg-slate-100 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-700"></asp:TextBox>
                        </div>
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Mobile</label>
                            <asp:TextBox ID="txtmobile" runat="server" ReadOnly="true"
                                CssClass="w-full bg-slate-100 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-700"></asp:TextBox>
                        </div>
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Shipping address *</label>
                            <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500"
                                placeholder="Enter your delivery address"
                                CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 focus:outline-none focus:border-rose-500"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div>
                    <h2 class="font-serif-heading text-xl font-bold text-slate-900 mb-4">Your items</h2>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Your cart is empty."
                        CssClass="w-full text-left text-sm text-slate-700 border-collapse">
                        <Columns>
                            <asp:BoundField DataField="Prod_Model" HeaderText="Product" />
                            <asp:BoundField DataField="Quentity" HeaderText="Quantity" />
                            <asp:BoundField DataField="Prod_Price" HeaderText="Unit price" DataFormatString="₹{0:N2}" />
                            <asp:BoundField DataField="Line_Total" HeaderText="Total" DataFormatString="₹{0:N2}" />
                        </Columns>
                        <HeaderStyle CssClass="border-b border-rose-100 text-slate-500 text-xs uppercase" />
                        <RowStyle CssClass="border-b border-slate-100" />
                        <EmptyDataRowStyle CssClass="py-8 text-center text-slate-500" />
                    </asp:GridView>
                </div>

                <div>
                    <h2 class="font-serif-heading text-xl font-bold text-slate-900 mb-3">Payment method</h2>
                    <asp:RadioButtonList ID="rblpay" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="rblpay_SelectedIndexChanged"
                        CssClass="flex flex-col sm:flex-row gap-4 text-sm text-slate-700">
                        <asp:ListItem Value="Cash on Delivery" Selected="True">Cash on delivery</asp:ListItem>
                        <asp:ListItem Value="UPI">UPI (payment status will remain pending)</asp:ListItem>
                    </asp:RadioButtonList>
                </div>

                <div class="ml-auto max-w-sm bg-rose-50 rounded-2xl p-5 space-y-2 text-sm">
                    <div class="flex justify-between"><span>Subtotal</span><span>₹<asp:Label ID="txtsubtotal" runat="server" Text="0.00"></asp:Label></span></div>
                    <div class="flex justify-between"><span>Delivery</span><span>₹<asp:Label ID="txtdelcharge" runat="server" Text="0.00"></asp:Label></span></div>
                    <div class="flex justify-between border-t border-rose-200 pt-2 font-bold text-base"><span>Grand total</span><span>₹<asp:Label ID="txtgrandtotal" runat="server" Text="0.00"></asp:Label></span></div>
                </div>

                <div class="flex flex-wrap gap-3">
                    <asp:Button ID="btnPlaceOrder" runat="server" Text="Place Order"
                        OnClick="btn_place_Click"
                        CssClass="bg-rose-600 hover:bg-rose-700 text-white font-bold px-7 py-3 rounded-xl text-sm shadow transition cursor-pointer" />
                    <a href="Add_To_Cart.aspx" class="bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold px-6 py-3 rounded-xl text-sm transition">Back to cart</a>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlConfirmation" runat="server" Visible="false"
                CssClass="bg-white rounded-3xl p-6 sm:p-8 border border-rose-100 shadow-xl space-y-6">
                <div class="text-center">
                    <h2 class="font-serif-heading text-2xl font-bold text-emerald-700">Order placed</h2>
                    <p class="text-sm text-slate-600 mt-1">Your order has been saved. Payment and fulfilment are pending.</p>
                </div>
                <div class="grid sm:grid-cols-2 gap-3 text-sm text-slate-700">
                    <p>Order number: <strong><asp:Label ID="lblOrderNo" runat="server"></asp:Label></strong></p>
                    <p>Order date: <strong><asp:Label ID="lblOrderDate" runat="server"></asp:Label></strong></p>
                    <p>Payment method: <strong><asp:Label ID="lblPayMethod" runat="server"></asp:Label></strong></p>
                    <p>Payment status: <strong><asp:Label ID="lblPayStatus" runat="server"></asp:Label></strong></p>
                    <p>Order status: <strong><asp:Label ID="lblOrderStatus" runat="server"></asp:Label></strong></p>
                    <p>Shipping address: <strong><asp:Label ID="lblShipAddress" runat="server"></asp:Label></strong></p>
                </div>
                <asp:GridView ID="GridViewConfirmation" runat="server" AutoGenerateColumns="False"
                    CssClass="w-full text-left text-sm text-slate-700 border-collapse">
                    <Columns>
                        <asp:BoundField DataField="Product_Name" HeaderText="Product" />
                        <asp:BoundField DataField="Quentity" HeaderText="Quantity" />
                        <asp:BoundField DataField="Unit_Price" HeaderText="Unit price" DataFormatString="₹{0:N2}" />
                        <asp:BoundField DataField="Line_Total" HeaderText="Total" DataFormatString="₹{0:N2}" />
                    </Columns>
                    <HeaderStyle CssClass="border-b border-rose-100 text-slate-500 text-xs uppercase" />
                    <RowStyle CssClass="border-b border-slate-100" />
                </asp:GridView>
                <div class="ml-auto max-w-sm space-y-2 text-sm">
                    <p class="flex justify-between"><span>Subtotal</span><strong>₹<asp:Label ID="lblSubtotal" runat="server"></asp:Label></strong></p>
                    <p class="flex justify-between"><span>Delivery</span><strong>₹<asp:Label ID="lblDelivery" runat="server"></asp:Label></strong></p>
                    <p class="flex justify-between border-t border-rose-200 pt-2 text-base"><span>Grand total</span><strong>₹<asp:Label ID="lblGrandTotal" runat="server"></asp:Label></strong></p>
                </div>
                <a href="Show_Products.aspx" class="inline-block bg-rose-600 hover:bg-rose-700 text-white font-bold px-6 py-3 rounded-xl text-sm transition">Continue shopping</a>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
