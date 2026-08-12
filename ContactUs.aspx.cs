using System;

namespace CakeShop
{
    public partial class ContactUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Clear form fields
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            ddlSubject.SelectedIndex = 0;
            txtMessage.Text = string.Empty;

            // Show success message
            pnlSuccess.Visible = true;
        }
    }
}
