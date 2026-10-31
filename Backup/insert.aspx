<%@ Page Title="User & Employee Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="insert.aspx.cs" Inherits="SweetDelights.insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="py-12 bg-rose-50/40 min-h-screen">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
            
            <!-- PAGE HEADER -->
            <div class="text-center max-w-2xl mx-auto space-y-2">
                <span class="text-xs font-bold uppercase tracking-widest text-rose-600 bg-rose-100 px-3.5 py-1 rounded-full border border-rose-200">
                    <i class="fa-solid fa-user-gear"></i> Admin & User Portal
                </span>
                <h1 class="font-serif-heading text-3xl sm:text-4xl font-bold text-slate-900">Employee & Customer Data Management</h1>
                <p class="text-xs sm:text-sm text-slate-600">Insert, Edit, Delete and View Users in SQL Database (`emp_tbl`).</p>
            </div>

            <!-- PANEL 1: LOGGED OUT STATE LINK -->
            <asp:Panel ID="pnlLoginLink" runat="server" CssClass="bg-white rounded-3xl p-8 border border-rose-100 shadow-xl max-w-lg mx-auto text-center space-y-4">
                <div class="w-16 h-16 bg-rose-100 text-rose-600 rounded-2xl flex items-center justify-center text-3xl mx-auto shadow-sm">
                    <i class="fa-solid fa-lock"></i>
                </div>
                <h3 class="font-serif-heading text-2xl font-bold text-slate-900">Admin Login Required</h3>
                <p class="text-slate-500 text-xs leading-relaxed">You must be logged in as Admin to access user records and database management tools.</p>
                <div class="pt-2">
                    <a href="Login.aspx" class="inline-block bg-rose-600 hover:bg-rose-700 text-white font-bold px-8 py-3.5 rounded-2xl shadow-lg transition text-xs">
                        <i class="fa-solid fa-right-to-bracket"></i> Click Here to Login
                    </a>
                </div>
            </asp:Panel>

            <!-- PANEL 2: LOGGED IN ADMIN PANEL & GRIDVIEW -->
            <asp:Panel ID="pnlAdminGrid" runat="server" Visible="false" CssClass="space-y-8">
                
                <!-- Welcome Banner -->
                <div class="bg-gradient-to-r from-rose-600 via-pink-600 to-amber-600 text-white rounded-3xl p-6 shadow-xl flex flex-col sm:flex-row items-center justify-between gap-4">
                    <div class="flex items-center gap-3">
                        <div class="w-12 h-12 rounded-2xl bg-white/20 flex items-center justify-center text-xl font-bold">
                            👋
                        </div>
                        <div>
                            <span class="text-xs uppercase tracking-wider text-rose-100 font-semibold">Admin Session Active</span>
                            <h2 class="font-serif-heading text-2xl font-bold">
                                <asp:Label ID="lblWelcome" runat="server" Text="Welcome Admin"></asp:Label>
                            </h2>
                        </div>
                    </div>
                </div>

                <!-- Registration / Edit Form Card -->
                <div class="bg-white rounded-3xl p-6 sm:p-8 border border-rose-100 shadow-xl space-y-6">
                    <div class="border-b border-rose-100 pb-4">
                        <h3 class="font-serif-heading text-xl font-bold text-slate-900">User / Employee Data Entry</h3>
                        <p class="text-xs text-slate-500">Fill in the fields to insert new record or update existing user.</p>
                    </div>

                    <div class="grid md:grid-cols-2 lg:grid-cols-3 gap-6 text-slate-800">
                        
                        <!-- Name Input -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Full Name (txtusername)</label>
                            <asp:TextBox ID="txtusername" runat="server" placeholder="Enter Full Name" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2.5 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium"></asp:TextBox>
                        </div>

                        <!-- Gender Radio Buttons -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Gender (rdogen)</label>
                            <asp:RadioButtonList ID="rdogen" runat="server" RepeatDirection="Horizontal" CssClass="flex gap-4 text-xs font-semibold pt-2 text-slate-700">
                                <asp:ListItem Value="Male">Male</asp:ListItem>
                                <asp:ListItem Value="Female">Female</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <!-- Email Input -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Email Address (txtemail)</label>
                            <asp:TextBox ID="txtemail" runat="server" TextMode="Email" placeholder="user@gmail.com" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2.5 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium"></asp:TextBox>
                        </div>

                        <!-- Password Input -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Password (txtpassword)</label>
                            <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" placeholder="••••••••" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2.5 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium"></asp:TextBox>
                        </div>

                        <!-- City Dropdown -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Select City (drpcity)</label>
                            <asp:DropDownList ID="drpcity" runat="server" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2.5 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium">
                                <asp:ListItem Value="">-- Select City --</asp:ListItem>
                                <asp:ListItem Value="Ahmedabad">Ahmedabad</asp:ListItem>
                                <asp:ListItem Value="Surat">Surat</asp:ListItem>
                                <asp:ListItem Value="Vadodara">Vadodara</asp:ListItem>
                                <asp:ListItem Value="Rajkot">Rajkot</asp:ListItem>
                                <asp:ListItem Value="Mumbai">Mumbai</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Mobile Input -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Mobile Number (txtmbl)</label>
                            <asp:TextBox ID="txtmbl" runat="server" placeholder="9876543210" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2.5 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium"></asp:TextBox>
                        </div>

                        <!-- Address Input (Full Width) -->
                        <div class="md:col-span-2">
                            <label class="block text-xs font-bold text-slate-700 mb-1">Address (txtaddress)</label>
                            <asp:TextBox ID="txtaddress" runat="server" TextMode="MultiLine" Rows="2" placeholder="Enter full address" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-2 text-xs text-slate-800 focus:outline-none focus:border-rose-500 font-medium"></asp:TextBox>
                        </div>

                        <!-- Image File Upload -->
                        <div>
                            <label class="block text-xs font-bold text-slate-700 mb-1">Upload Photo (imgupload)</label>
                            <asp:FileUpload ID="imgupload" runat="server" CssClass="w-full bg-slate-50 border border-slate-200 rounded-xl px-3 py-2 text-xs text-slate-700 font-medium" />
                        </div>

                    </div>

                    <!-- Action Buttons -->
                    <div class="pt-4 flex items-center gap-4">
                        <asp:Button ID="Save_btn" runat="server" Text="Save" OnClick="Save_btn_Click" CssClass="bg-rose-600 hover:bg-rose-700 text-white font-bold px-8 py-3 rounded-xl text-xs shadow-md transition cursor-pointer" />
                        <button type="button" onclick="window.location.reload();" class="bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold px-6 py-3 rounded-xl text-xs transition">Reset Form</button>
                    </div>

                </div>

                <!-- Database Records GridView -->
                <div class="bg-white rounded-3xl p-6 sm:p-8 border border-rose-100 shadow-xl space-y-4">
                    <div class="flex items-center justify-between border-b border-rose-100 pb-3">
                        <h3 class="font-serif-heading text-xl font-bold text-slate-900">Database Records (`emp_tbl`)</h3>
                        <span class="text-xs font-semibold text-rose-600 bg-rose-50 px-3 py-1 rounded-full">Live SQL Data</span>
                    </div>

                    <div class="overflow-x-auto">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" CssClass="w-full text-left text-xs text-slate-700 border-collapse">
                            <HeaderStyle CssClass="bg-slate-900 text-white font-bold uppercase tracking-wider py-3 px-4" />
                            <RowStyle CssClass="border-b border-slate-100 hover:bg-rose-50/50 transition py-3 px-4" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" />
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <img src='<%# Eval("Image") %>' alt="User Image" class="w-10 h-10 rounded-full object-cover border border-rose-200" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="flex items-center gap-2">
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="cmd_edt" CommandArgument='<%# Eval("Id") %>' CssClass="bg-amber-100 text-amber-800 font-bold px-3 py-1.5 rounded-lg text-[11px] hover:bg-amber-200 transition">
                                                <i class="fa-solid fa-pen"></i> Edit
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="cmd_del" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Are you sure you want to delete this record?');" CssClass="bg-rose-100 text-rose-700 font-bold px-3 py-1.5 rounded-lg text-[11px] hover:bg-rose-200 transition">
                                                <i class="fa-solid fa-trash"></i> Delete
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </asp:Panel>

        </div>
    </div>
</asp:Content>
