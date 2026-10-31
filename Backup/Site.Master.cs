using System;
using System.Web.UI;

namespace SweetDelights
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckUserSession();
            }
        }

        private void CheckUserSession()
        {
            if (Session["User"] != null)
            {
                User user = (User)Session["User"];
                lblUserGreeting.Text = "Hello, " + user.FullName;
                pnlUserLoggedIn.Visible = true;
                pnlUserLoggedOut.Visible = false;
            }
            else
            {
                pnlUserLoggedIn.Visible = false;
                pnlUserLoggedOut.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }
    }
}
