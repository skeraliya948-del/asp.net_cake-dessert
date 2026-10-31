<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Add_To_Cart.aspx.cs" Inherits="SweetDelights.Add_To_Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-12 bg-rose-50/40 min-h-screen">
        <div class="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
            <div class="bg-gradient-to-r from-rose-600 via-pink-600 to-amber-600 text-white rounded-3xl p-6 sm:p-8 shadow-xl">
                <span class="text-xs uppercase tracking-wider text-rose-100 font-semibold">Your Basket</span>
                <h1 class="font-serif-heading text-2xl font-bold">
                    <asp:Label ID="Label5" runat="server" Text="Your Shopping Cart"></asp:Label>
                </h1>
            </div>

            <div class="bg-white rounded-3xl p-6 sm:p-8 border border-rose-100 shadow-xl space-y-6">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="Cart_Id" OnRowCommand="GridView1_RowCommand"
                    OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
                    OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound"
                    EmptyDataText="Your cart is empty. Browse our catalog to add products."
                    CssClass="w-full text-left text-sm text-slate-700 border-collapse">
                    <Columns>
                        <asp:ImageField DataImageUrlField="Prod_Img" HeaderText="Product"
                            ControlStyle-Width="72px" ControlStyle-Height="72px"
                            ControlStyle-CssClass="rounded-xl object-cover" />
                        <asp:BoundField DataField="Prod_Model" HeaderText="Product" ReadOnly="True" />
                        <asp:BoundField DataField="Prod_Price" HeaderText="Price"
                            DataFormatString="₹{0:N2}" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <%#: Eval("Quentity") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownList1" runat="server"
                                    CssClass="border border-slate-200 rounded-lg px-2 py-1">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" EditText="Change quantity"
                            UpdateText="Save" CancelText="Cancel" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="removeButton" runat="server" Text="Remove"
                                    CommandName="cmd_remove" CommandArgument='<%# Eval("Cart_Id") %>'
                                    CausesValidation="False" CssClass="text-rose-600 font-semibold" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="border-b border-rose-100 text-slate-500 text-xs uppercase" />
                    <RowStyle CssClass="border-b border-slate-100" />
                    <EmptyDataRowStyle CssClass="py-8 text-center text-slate-500" />
                </asp:GridView>

                <div class="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 border-t border-rose-100 pt-5">
                    <asp:Label ID="grand_total" runat="server" CssClass="text-xl font-bold text-slate-900"
                        Text="Grand Total: ₹0.00"></asp:Label>
                    <asp:Button ID="btnCheckout" runat="server" Text="Continue to Checkout"
                        OnClick="btnCheckout_Click"
                        CssClass="bg-rose-600 hover:bg-rose-700 text-white font-bold px-6 py-3 rounded-xl text-sm shadow transition cursor-pointer" />
                </div>
                <div>
                    <a href="Show_Products.aspx" class="text-sm font-semibold text-rose-600 hover:text-rose-700">Continue shopping</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
