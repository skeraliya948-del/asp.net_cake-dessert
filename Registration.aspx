<%@ Page Language="C#" AutoEventWireup="true" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<script runat="server">

    SqlConnection con;
    SqlDataAdapter da;
    DataSet ds;
    SqlCommand cmd;
    string fnm = "";

    string s = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CakeShopDB;Integrated Security=True";

    void getcon()
    {
        con = new SqlConnection(s);
        con.Open();
    }

    void imgupload()
    {
        if (flpimg.HasFile)
        {
            string folder = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            fnm = "Uploads/" + flpimg.FileName;
            flpimg.SaveAs(Server.MapPath(fnm));
        }
    }

    void fillgrid()
    {
        try
        {
            getcon();
            da = new SqlDataAdapter("SELECT * FROM Registration", con);
            ds = new DataSet();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "&#9888; DB Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
        }
    }

    void filldata()
    {
        getcon();
        da = new SqlDataAdapter("SELECT * FROM Registration WHERE Id='" + ViewState["id"] + "'", con);
        ds = new DataSet();
        da.Fill(ds);

        txtnm.Text  = ds.Tables[0].Rows[0][1].ToString();
        txteml.Text = ds.Tables[0].Rows[0][3].ToString();
        drpct.SelectedValue = ds.Tables[0].Rows[0][4].ToString();
        txtadd.Text = ds.Tables[0].Rows[0][5].ToString();
        txtmbl.Text = ds.Tables[0].Rows[0][6].ToString();

        if (ds.Tables[0].Rows[0]["Gender"].ToString() == "Male")
            rdbgen.SelectedValue = "Male";
        else
            rdbgen.SelectedValue = "Female";

        con.Close();
    }

    void clear()
    {
        txtnm.Text  = "";
        txteml.Text = "";
        txtadd.Text = "";
        txtmbl.Text = "";
        rdbgen.SelectedIndex = -1;
        drpct.SelectedIndex  = 0;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            fillgrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {
            try
            {
                imgupload();
                getcon();
                cmd = new SqlCommand(
                    "INSERT INTO Registration(Name,Gender,Email,City,Address,Mobile,ImagePath) " +
                    "VALUES('" + txtnm.Text + "','" +
                    rdbgen.SelectedValue + "','" +
                    txteml.Text + "','" +
                    drpct.SelectedValue + "','" +
                    txtadd.Text + "','" +
                    txtmbl.Text + "','" +
                    fnm + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                lblMessage.Text = "&#10003; Registration saved successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                clear();
                fillgrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "&#10060; Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            try
            {
                getcon();
                cmd = new SqlCommand(
                    "UPDATE Registration SET " +
                    "Name='"    + txtnm.Text + "'," +
                    "Gender='"  + rdbgen.SelectedValue + "'," +
                    "Email='"   + txteml.Text + "'," +
                    "City='"    + drpct.SelectedValue + "'," +
                    "Address='" + txtadd.Text + "'," +
                    "Mobile='"  + txtmbl.Text + "' " +
                    "WHERE Id='" + ViewState["id"] + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                lblMessage.Text = "&#10003; Record updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                clear();
                btnSave.Text = "Save";
                fillgrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "&#10060; Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "cmd_edt")
        {
            ViewState["id"] = Convert.ToInt32(e.CommandArgument);
            btnSave.Text = "Update";
            filldata();
            lblMessage.Text = "";
        }
        else if (e.CommandName == "cmd_dlt")
        {
            try
            {
                getcon();
                cmd = new SqlCommand(
                    "DELETE FROM Registration WHERE Id='" + e.CommandArgument + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                lblMessage.Text = "Record deleted.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
                fillgrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "&#10060; Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

</script>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Register – Sweet Layers</title>
    <link rel="stylesheet" href="Styles/main.css" />
</head>
<body>

    <!-- ===== NAVBAR ===== -->
    <nav class="navbar">
        <div class="brand">&#127874; Sweet<span>Layers</span></div>
        <ul>
            <li><a href="index.html">Home</a></li>
            <li><a href="index.html">Cake Menu</a></li>
            <li><a href="index.html">About</a></li>
            <li><a href="Registration.aspx" class="active">Register</a></li>
            <li><a href="index.html">Contact Us</a></li>
        </ul>
    </nav>

    <div style="background:#fff9f2; min-height:80vh; padding:30px 16px;">
    <form id="form1" runat="server">

        <!-- ===== FORM BOX ===== -->
        <div class="reg-wrapper">
            <div class="reg-header">
                <h2>&#128221; Customer Registration</h2>
                <p>Fill in your details to register with Sweet Layers</p>
            </div>
            <div class="reg-form">

                <asp:Label ID="lblMessage" runat="server"
                    Style="display:block;margin-bottom:14px;font-size:0.95rem;font-weight:600;"></asp:Label>

                <div class="form-group">
                    <label>Name</label>
                    <div class="input-wrap">
                        <asp:TextBox ID="txtnm" runat="server" CssClass="textbox" placeholder="Enter your full name"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label>Gender</label>
                    <div class="input-wrap" style="padding-top:6px;">
                        <asp:RadioButtonList ID="rdbgen" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="Female">Female &nbsp;&nbsp;</asp:ListItem>
                            <asp:ListItem Value="Male">Male</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <div class="form-group">
                    <label>Email</label>
                    <div class="input-wrap">
                        <asp:TextBox ID="txteml" runat="server" CssClass="textbox" placeholder="your@email.com"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label>City</label>
                    <div class="input-wrap">
                        <asp:DropDownList ID="drpct" runat="server" CssClass="textbox">
                            <asp:ListItem>-- Select City --</asp:ListItem>
                            <asp:ListItem>Rajkot</asp:ListItem>
                            <asp:ListItem>Mumbai</asp:ListItem>
                            <asp:ListItem>Pune</asp:ListItem>
                            <asp:ListItem>Chennai</asp:ListItem>
                            <asp:ListItem>Ahemdabad</asp:ListItem>
                            <asp:ListItem>Udaipur</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <label>Address</label>
                    <div class="input-wrap">
                        <asp:TextBox ID="txtadd" runat="server" CssClass="textbox"
                            TextMode="MultiLine" Rows="3" placeholder="Enter your full address"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label>Mobile</label>
                    <div class="input-wrap">
                        <asp:TextBox ID="txtmbl" runat="server" CssClass="textbox"
                            placeholder="10-digit mobile number" MaxLength="10"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label>Image</label>
                    <div class="input-wrap">
                        <asp:FileUpload ID="flpimg" runat="server" />
                        <small style="color:#999;font-size:0.8rem;">Optional – JPG/PNG</small>
                    </div>
                </div>

                <div class="form-group">
                    <label></label>
                    <div class="input-wrap">
                        <asp:Button ID="btnSave" runat="server" Text="Save"
                            CssClass="btn-submit" OnClick="btnSave_Click" />
                    </div>
                </div>

            </div>
        </div>

        <!-- ===== GRIDVIEW ===== -->
        <div class="gridview-section" style="margin-top:40px;">
            <h3>&#128203; Registered Users</h3>
            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="False"
                CssClass="GridViewStyle"
                EmptyDataText="No registrations yet."
                OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Id">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gender">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("Gender") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mobile">
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Image">
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" Height="50" Width="50"
                                ImageUrl='<%# Eval("ImagePath") %>'
                                style="border-radius:6px;object-fit:cover;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server"
                                CommandArgument='<%# Eval("Id") %>'
                                CommandName="cmd_edt"
                                Style="color:#bf6b1a;font-weight:600;">&#9998; Edit</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server"
                                CommandArgument='<%# Eval("Id") %>'
                                CommandName="cmd_dlt"
                                Style="color:#e74c3c;font-weight:600;"
                                OnClientClick="return confirm('Delete this record?');">&#128465; Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </form>
    </div>

    <!-- ===== FOOTER ===== -->
    <footer class="footer">
        <p>&copy; 2024 Sweet Layers Cake Shop. Made with <span>&#10084;</span> for cake lovers.</p>
    </footer>

</body>
</html>
