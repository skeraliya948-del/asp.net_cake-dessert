<%@ Page Language="C#" AutoEventWireup="true" ResponseEncoding="UTF-8" ContentType="text/html; charset=utf-8" %>

<script runat="server">
    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        txtName.Text             = string.Empty;
        txtEmail.Text            = string.Empty;
        txtPhone.Text            = string.Empty;
        ddlSubject.SelectedIndex = 0;
        txtMessage.Text          = string.Empty;

        pnlSuccess.Visible = true;
    }
</script>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Contact Us – Sweet Layers</title>
    <link rel="stylesheet" href="Styles/main.css" />
</head>
<body>

    <!-- ===== NAVBAR ===== -->
    <nav class="navbar">
        <div class="brand">&#127874; Sweet<span>Layers</span></div>
        <ul>
            <li><a href="index.html">Home</a></li>
            <li><a href="index.html" onclick="sessionStorage.setItem('page','menu')">Cake Menu</a></li>
            <li><a href="index.html" onclick="sessionStorage.setItem('page','about')">About</a></li>
            <li><a href="Registration.aspx">Register</a></li>
            <li><a href="ContactUs.aspx" class="active">Contact Us</a></li>
        </ul>
    </nav>

    <!-- ===== HEADER ===== -->
    <div class="contact-header">
        <h1>&#128222; Contact Us</h1>
        <p>We'd love to hear from you! Reach out for orders, queries, or just to say hello.</p>
    </div>

    <!-- ===== FORM + INFO ===== -->
    <form id="form1" runat="server">
    <div class="contact-page-wrap">

        <!-- CONTACT FORM -->
        <div class="contact-form-box">
            <h2>Send Us a Message</h2>

            <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert-success-contact" Visible="false">
                &#10004; Thank you! Your message has been sent. We'll get back to you shortly.
            </asp:Panel>

            <div class="form-group">
                <label>Full Name</label>
                <div class="input-wrap">
                    <asp:TextBox ID="txtName" runat="server" CssClass="textbox" placeholder="Your full name" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName"
                        ErrorMessage="Name is required." ForeColor="Red" Display="Dynamic"
                        Style="font-size:0.82rem;" />
                </div>
            </div>

            <div class="form-group">
                <label>Email</label>
                <div class="input-wrap">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" TextMode="Email" placeholder="your@email.com" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email is required." ForeColor="Red" Display="Dynamic"
                        Style="font-size:0.82rem;" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        ErrorMessage="Enter a valid email." ForeColor="Red" Display="Dynamic"
                        Style="font-size:0.82rem;" />
                </div>
            </div>

            <div class="form-group">
                <label>Phone</label>
                <div class="input-wrap">
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" placeholder="10-digit mobile number" MaxLength="10" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone"
                        ValidationExpression="^\d{10}$"
                        ErrorMessage="Enter a valid 10-digit number." ForeColor="Red" Display="Dynamic"
                        Style="font-size:0.82rem;" />
                </div>
            </div>

            <div class="form-group">
                <label>Subject</label>
                <div class="input-wrap">
                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="textbox">
                        <asp:ListItem Value="">-- Select Subject --</asp:ListItem>
                        <asp:ListItem Value="Order Enquiry">Order Enquiry</asp:ListItem>
                        <asp:ListItem Value="Custom Cake Design">Custom Cake Design</asp:ListItem>
                        <asp:ListItem Value="Delivery Information">Delivery Information</asp:ListItem>
                        <asp:ListItem Value="Feedback">Feedback</asp:ListItem>
                        <asp:ListItem Value="Other">Other</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSubject"
                        InitialValue="" ErrorMessage="Please select a subject."
                        ForeColor="Red" Display="Dynamic" Style="font-size:0.82rem;" />
                </div>
            </div>

            <div class="form-group">
                <label>Message</label>
                <div class="input-wrap">
                    <asp:TextBox ID="txtMessage" runat="server" CssClass="textbox" TextMode="MultiLine"
                        Rows="5" placeholder="Write your message here..." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMessage"
                        ErrorMessage="Message cannot be empty." ForeColor="Red" Display="Dynamic"
                        Style="font-size:0.82rem;" />
                </div>
            </div>

            <div style="text-align:center; margin-top:10px;">
                <asp:Button ID="btnSend" runat="server" Text="Send Message &#9993;"
                    CssClass="btn-submit" OnClick="btnSend_Click" />
            </div>
        </div>

        <!-- INFO CARDS -->
        <div class="contact-info-col">
            <div class="contact-info-card">
                <div class="ci-icon">&#128205;</div>
                <div><h4>Visit Us</h4><p>12, Baker Street<br />MG Road, Bengaluru<br />Karnataka – 560001</p></div>
            </div>
            <div class="contact-info-card">
                <div class="ci-icon">&#128222;</div>
                <div><h4>Call Us</h4><p>+91 98765 43210<br />Mon – Sat: 9 AM – 8 PM</p></div>
            </div>
            <div class="contact-info-card">
                <div class="ci-icon">&#128140;</div>
                <div><h4>Email Us</h4><p>hello@sweetlayers.in<br />orders@sweetlayers.in</p></div>
            </div>
            <div class="contact-info-card">
                <div class="ci-icon">&#128337;</div>
                <div><h4>Working Hours</h4><p>Mon – Fri: 9 AM – 9 PM<br />Saturday: 9 AM – 7 PM<br />Sunday: 10 AM – 5 PM</p></div>
            </div>
        </div>

    </div>
    </form>

    <!-- ===== FOOTER ===== -->
    <footer class="footer">
        <p>&copy; 2024 Sweet Layers Cake Shop. Made with <span>&#10084;</span> for cake lovers.</p>
    </footer>

</body>
</html>
