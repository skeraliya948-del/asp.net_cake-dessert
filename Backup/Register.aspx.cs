using System;

namespace SweetDelights
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                lblRegisterMessage.Text = "Please fill in all required fields.";
                lblRegisterMessage.CssClass = "block text-xs font-bold text-rose-600 bg-rose-50 p-3 rounded-xl border border-rose-200 mt-2";
                return;
            }

            if (password != confirmPassword)
            {
                lblRegisterMessage.Text = "Passwords do not match!";
                lblRegisterMessage.CssClass = "block text-xs font-bold text-rose-600 bg-rose-50 p-3 rounded-xl border border-rose-200 mt-2";
                return;
            }

            // Create registered user session
            User newUser = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                IsLoggedIn = true
            };

            Session["User"] = newUser;
            Response.Redirect("Default.aspx");
        }
    }
}
